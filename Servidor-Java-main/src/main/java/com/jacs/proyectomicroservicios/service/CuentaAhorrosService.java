package com.jacs.proyectomicroservicios.service;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import com.jacs.proyectomicroservicios.model.Movimiento;
import com.jacs.proyectomicroservicios.observer.CuentaListSubject;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.stream.Collectors;

@Service
public class CuentaAhorrosService implements ICuentaAhorrosService {

    private static CuentaAhorrosService cuentaService;

    private CuentaAhorrosService() {
    }

    public static CuentaAhorrosService getInstance(){
        if(cuentaService == null){
            cuentaService = new CuentaAhorrosService();
        }
        return cuentaService;
    }

    private final List<CuentaAhorros> cuentas = new ArrayList<>();

    @Override
    public void agregar(CuentaAhorros cuenta) {
        if (cuenta == null) {
            throw new IllegalArgumentException("La cuenta no puede ser nula");
        }

        boolean existe = cuentas.stream()
                .anyMatch(c -> c.getNumeroCuenta() == cuenta.getNumeroCuenta());

        if (existe) {
            throw new IllegalArgumentException("Ya existe una cuenta con el número: " + cuenta.getNumeroCuenta());
        }

        cuenta.setEstado("Activo");
        cuentas.add(cuenta);

        // Notificar observers de la lista tras inserción
        CuentaListSubject.notifyObservers(listar());
    }

    @Override
    public CuentaAhorros buscarPorNumero(int numeroCuenta) {
        return cuentas.stream()
                .filter(c -> c.getNumeroCuenta() == numeroCuenta)
                .findFirst()
                .orElseThrow(() -> new NoSuchElementException("Cuenta no encontrada: " + numeroCuenta));
    }

    @Override
    public Movimiento agregarMovimiento(int numeroCuenta, Movimiento datos) {
        CuentaAhorros cuenta = buscarPorNumero(numeroCuenta);

        int nextId = cuenta.getMovimientos()
                .stream()
                .mapToInt(Movimiento::getId)
                .max()
                .orElse(0) + 1;

        Movimiento movimiento = new Movimiento(
                nextId,
                LocalDateTime.now(),
                datos.getMonto(),
                datos.getTipo(),
                numeroCuenta
        );

        cuenta.getMovimientos().add(movimiento);

        // Actualizar saldo según tipo de movimiento
        if ("CREDITO".equalsIgnoreCase(datos.getTipo())) {
            cuenta.setSaldo(cuenta.getSaldo() + datos.getMonto());
        } else if ("DEBITO".equalsIgnoreCase(datos.getTipo())) {
            cuenta.setSaldo(cuenta.getSaldo() - datos.getMonto());
        }

        return movimiento;
    }

    @Override
    public List<Movimiento> listarMovimientos(int numeroCuenta) {
        CuentaAhorros cuenta = buscarPorNumero(numeroCuenta);
        return cuenta.getMovimientos();
    }

    @Override
    public List<CuentaAhorros> buscarPorTitular(String titular) {
        return cuentas.stream()
                .filter(c -> c.getTitular().toLowerCase().contains(titular.toLowerCase()))
                .sorted(Comparator.comparingInt(CuentaAhorros::getNumeroCuenta))
                .collect(Collectors.toList());
    }

    @Override
    public List<CuentaAhorros> listar() {
        return cuentas.stream()
                .sorted(Comparator.comparingInt(CuentaAhorros::getNumeroCuenta))
                .collect(Collectors.toList());
    }

    @Override
    public List<CuentaAhorros> listarConFiltro(String titular, String estado) {
        return cuentas.stream()
                .filter(c -> titular == null || titular.isBlank()
                        || c.getTitular().toLowerCase().contains(titular.toLowerCase()))
                .filter(c -> estado == null || estado.isBlank()
                        || c.getEstado().equalsIgnoreCase(estado))
                .sorted(Comparator.comparingInt(CuentaAhorros::getNumeroCuenta))
                .collect(Collectors.toList());
    }

    @Override
    public boolean eliminar(int numeroCuenta) {
        CuentaAhorros cuenta = buscarPorNumero(numeroCuenta);

        if ("Activo".equalsIgnoreCase(cuenta.getEstado())) {
            cuenta.setEstado("Inactivo");

            // Notificar observers de la lista tras cambio
            CuentaListSubject.notifyObservers(listar());
            return true;
        }

        return false;
    }

    @Override
    public CuentaAhorros actualizar(int numeroCuenta, CuentaAhorros datos) {
        CuentaAhorros cuenta = buscarPorNumero(numeroCuenta);

        if (datos.getTitular() != null && !datos.getTitular().trim().isEmpty()) {
            cuenta.setTitular(datos.getTitular());
        }

        if (datos.getSaldo() >= 200000.0) {
            cuenta.setSaldo(datos.getSaldo());
        }

        if (datos.getTasaInteres() > 0) {
            cuenta.setTasaInteres(datos.getTasaInteres());
        }

        if (datos.getEstado() != null && !datos.getEstado().trim().isEmpty()) {
            cuenta.setEstado(datos.getEstado());
        }

        // Notificar observers de la lista tras actualización
        CuentaListSubject.notifyObservers(listar());
        return cuenta;
    }
}
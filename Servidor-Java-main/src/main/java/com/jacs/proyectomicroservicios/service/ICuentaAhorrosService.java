package com.jacs.proyectomicroservicios.service;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import com.jacs.proyectomicroservicios.model.Movimiento;

import java.util.List;

public interface ICuentaAhorrosService {

    void agregar(CuentaAhorros cuenta);

    CuentaAhorros buscarPorNumero(int numeroCuenta);

    Movimiento agregarMovimiento(int numeroCuenta, Movimiento datos);

    List<Movimiento> listarMovimientos(int numeroCuenta);

    List<CuentaAhorros> buscarPorTitular(String titular);

    List<CuentaAhorros> listar();

    List<CuentaAhorros> listarConFiltro(String titular, String estado);

    boolean eliminar(int numeroCuenta);

    CuentaAhorros actualizar(int numeroCuenta, CuentaAhorros datos);
}
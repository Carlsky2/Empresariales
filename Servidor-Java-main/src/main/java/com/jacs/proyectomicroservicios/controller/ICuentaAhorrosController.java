package com.jacs.proyectomicroservicios.controller;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import com.jacs.proyectomicroservicios.model.Movimiento;
import org.springframework.http.ResponseEntity;
import java.util.List;

public interface ICuentaAhorrosController {

    // Healthcheck
    ResponseEntity<String> healthCheck();

    // Crear cuenta
    ResponseEntity<?> crear(CuentaAhorros cuenta);

    // Agregar movimiento
    ResponseEntity<?> agregarMovimiento(int numero, Movimiento datos);

    // Listar movimientos
    ResponseEntity<List<Movimiento>> listarMovimientos(int numero);

    // Listar cuentas
    ResponseEntity<List<CuentaAhorros>> listar(String titular, String estado);

    // Filtrar cuentas
    ResponseEntity<List<CuentaAhorros>> filtrar(String titular, String estado);

    // Buscar por titular
    ResponseEntity<List<CuentaAhorros>> buscarPorTitular(String titular);

    // Buscar por número
    ResponseEntity<?> buscarPorNumero(int numero);

    // Eliminar cuenta
    ResponseEntity<?> eliminar(int numero);

    // Actualizar cuenta
    ResponseEntity<?> actualizar(int numero, CuentaAhorros datos);
}

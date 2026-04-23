package com.jacs.proyectomicroservicios.controller;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import com.jacs.proyectomicroservicios.service.CuentaAhorrosService;
import com.jacs.proyectomicroservicios.service.ICuentaAhorrosService;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.NoSuchElementException;
import com.jacs.proyectomicroservicios.model.Movimiento;

@RestController
@RequestMapping("/cuentas")
public class CuentaAhorrosController implements ICuentaAhorrosController {


    private final ICuentaAhorrosService service;

    public CuentaAhorrosController() {
        this.service = CuentaAhorrosService.getInstance();
    }

    // GET /cuentas/healthcheck
    @GetMapping("/healthcheck")
    public ResponseEntity<String> healthCheck() {
        return ResponseEntity.ok("Servicio CuentaAhorros Ok!");
    }

    @GetMapping("/movimientos")
    public ResponseEntity<List<Movimiento>> listarTodosMovimientos() {
        return ResponseEntity.ok(
                service.listarTodosMovimientos()
        );
    }

    // POST /cuentas  → Insertar
    @PostMapping
    public ResponseEntity<?> crear(@RequestBody CuentaAhorros cuenta) {
        try {
            service.agregar(cuenta);
            return ResponseEntity.status(HttpStatus.CREATED).body(cuenta);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    // MOVIMIENTOS: añadir movimiento a una cuenta existente
    @PostMapping("/{numero}/movimientos")
    public ResponseEntity<?> agregarMovimiento(@PathVariable int numero,
                                              @RequestBody Movimiento datos) {
        try {
            Movimiento m = service.agregarMovimiento(numero, datos);
            return ResponseEntity.status(HttpStatus.CREATED).body(m);
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(e.getMessage());
        }
    }

    // MOVIMIENTOS: listar movimientos de una cuenta
    @GetMapping("/{numero}/movimientos")
    public ResponseEntity<List<Movimiento>> listarMovimientos(@PathVariable int numero) {
        try {
            return ResponseEntity.ok(service.listarMovimientos(numero));
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(null);
        }
    }

    // GET /cuentas  → Listar todos (con filtros opcionales)
    // Soporta ?titular=...&estado=... para cumplir la rúbrica "Listar XXX con parámetro"
    @GetMapping
    public ResponseEntity<List<CuentaAhorros>> listar(
            @RequestParam(required = false) String titular,
            @RequestParam(required = false) String estado) {
        // Si no se proporcionan filtros, listar todo
        if ((titular == null || titular.isBlank()) && (estado == null || estado.isBlank())) {
            return ResponseEntity.ok(service.listar());
        }
        // De lo contrario, listar con filtro (titular y/o estado)
        return ResponseEntity.ok(service.listarConFiltro(titular, estado));
    }

    // GET /cuentas/filtrar?titular=X&estado=Y  → Listar con filtro (server-side)
    @GetMapping("/filtrar")
    public ResponseEntity<List<CuentaAhorros>> filtrar(
            @RequestParam(required = false) String titular,
            @RequestParam(required = false) String estado) {
        return ResponseEntity.ok(service.listarConFiltro(titular, estado));
    }

    // GET /cuentas/buscar?titular=X  → Buscar por titular
    @GetMapping("/buscar")
    public ResponseEntity<List<CuentaAhorros>> buscarPorTitular(
            @RequestParam String titular) {
        return ResponseEntity.ok(service.buscarPorTitular(titular));
    }

    // GET /cuentas/{numero}  → Buscar por número
    @GetMapping("/{numero}")
    public ResponseEntity<?> buscarPorNumero(@PathVariable int numero) {
        try {
            return ResponseEntity.ok(service.buscarPorNumero(numero));
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(e.getMessage());
        }
    }

    // DELETE /cuentas/{numero}  → Eliminar (marca como Inactivo)
    @DeleteMapping("/{numero}")
    public ResponseEntity<?> eliminar(@PathVariable int numero) {
        try {
            boolean resultado = service.eliminar(numero);
            if (resultado) {
                return ResponseEntity.ok("Cuenta " + numero + " eliminada (Inactivo)");
            }
            return ResponseEntity.badRequest().body("La cuenta ya estaba inactiva");
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(e.getMessage());
        }
    }

    // PUT /cuentas/{numero}  → Actualizar
    @PutMapping("/{numero}")
    public ResponseEntity<?> actualizar(@PathVariable int numero,
                                        @RequestBody CuentaAhorros datos)
    {
        try {
            CuentaAhorros actualizada = service.actualizar(numero, datos);
            return ResponseEntity.ok(actualizada);
        } catch (NoSuchElementException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(e.getMessage());
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
}

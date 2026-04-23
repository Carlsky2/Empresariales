package com.jacs.proyectomicroservicios.model;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;

@Data
@Builder
public class Movimiento {
    // Identificador del movimiento dentro de la cuenta
    private int id;
    private LocalDateTime fechaMovimiento;
    private double monto;
    private String tipo; // EJ: CREDITO, DEBITO
    // Referencia a la cuenta asociada (master)
    private int numeroCuenta;

    public Movimiento() {
    }

    public Movimiento(int id, LocalDateTime fechaMovimiento, double monto, String tipo, int numeroCuenta) {
        this.id = id;
        this.fechaMovimiento = fechaMovimiento;
        this.monto = monto;
        this.tipo = tipo;
        this.numeroCuenta = numeroCuenta;
    }
}

package com.jacs.proyectomicroservicios.model;

import com.fasterxml.jackson.annotation.JsonFormat;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Data
@Builder
@AllArgsConstructor
public class CuentaAhorros {

    private int numeroCuenta;
    private String titular;
    private double saldo;
    private String estado;
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd'T'HH:mm:ss")
    private LocalDateTime fechaApertura;
    private double tasaInteres;
    // Lista de movimientos asociados (Master-Detail)
    private List<Movimiento> movimientos = new ArrayList<>();

    public CuentaAhorros() {
    }

    public CuentaAhorros(int numeroCuenta, String titular, double saldo,
                         double tasaInteres, LocalDateTime fechaApertura)
    {
        if (numeroCuenta < 0) {
            throw new IllegalArgumentException("El número de cuenta no puede ser negativo");
        }
        if (titular == null || titular.trim().isEmpty()) {
            throw new IllegalArgumentException("El titular no puede estar vacío");
        }
        if (saldo < 200000.0) {
            throw new IllegalArgumentException("El saldo inicial mínimo es $200.000");
        }
        this.numeroCuenta = numeroCuenta;
        this.titular = titular;
        this.saldo = saldo;
        this.tasaInteres = tasaInteres;
        this.fechaApertura = fechaApertura;
        this.estado = "Activo";
    }

    public double calcularCostoMensual() {
        return saldo * 0.005;
    }

    public double calcularRendimiento() {
        return saldo * tasaInteres;
    }
}

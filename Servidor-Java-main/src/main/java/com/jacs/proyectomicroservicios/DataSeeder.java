package com.jacs.proyectomicroservicios;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import com.jacs.proyectomicroservicios.service.CuentaAhorrosService;
import org.springframework.boot.CommandLineRunner;
import org.springframework.stereotype.Component;

@Component
public class DataSeeder implements CommandLineRunner {

    private final CuentaAhorrosService service;

    public DataSeeder(CuentaAhorrosService service) {
        this.service = service;
    }

    @Override
    public void run(String... args) {
    }
}
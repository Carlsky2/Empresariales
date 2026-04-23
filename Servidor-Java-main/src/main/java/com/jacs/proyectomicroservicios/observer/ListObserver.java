package com.jacs.proyectomicroservicios.observer;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import java.util.List;

public interface ListObserver {
    void onListUpdated(List<CuentaAhorros> cuentas);
}

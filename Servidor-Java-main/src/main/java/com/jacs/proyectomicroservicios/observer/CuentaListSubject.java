package com.jacs.proyectomicroservicios.observer;

import com.jacs.proyectomicroservicios.model.CuentaAhorros;
import java.util.ArrayList;
import java.util.List;

public class CuentaListSubject {
    private static final List<ListObserver> observadores = new ArrayList<>();

    public static void attach(ListObserver observer) {
        if (!observadores.contains(observer)) {
            observadores.add(observer);
        }
    }

    public static void detach(ListObserver observer) {
        observadores.remove(observer);
    }

    public static void notifyObservers(List<CuentaAhorros> cuentas) {
        for (ListObserver observer : observadores) {
            observer.onListUpdated(cuentas);
        }
    }
}
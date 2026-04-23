const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('api', {
  //Cuentas
  healthcheck: () => ipcRenderer.invoke('healthcheck'),
  crearCuenta: (cuenta) => ipcRenderer.invoke('crear-cuenta', cuenta),
  listarCuentas: () => ipcRenderer.invoke('listar-cuentas'),
  filtrarCuentas: (titular, estado) => ipcRenderer.invoke('filtrar-cuentas', titular, estado),
  buscarCuenta: (numero) => ipcRenderer.invoke('buscar-cuenta', numero),
  buscarPorTitular: (titular) => ipcRenderer.invoke('buscar-por-titular', titular),
  actualizarCuenta: (numero, cuenta) => ipcRenderer.invoke('actualizar-cuenta', numero, cuenta),
  eliminarCuenta: (numero) => ipcRenderer.invoke('eliminar-cuenta', numero),
  
  //Movimientos
  agregarMovimiento: (numero, monto, tipo) => ipcRenderer.invoke('agregar-movimiento', numero, monto, tipo),
  listarMovimientos: (numero) => ipcRenderer.invoke('listar-movimientos', numero),

  // ============== OBSERVER PATTERN (en renderer) ==============
  onActualizacion: (callback) => ipcRenderer.on('actualizacion', (e, data) => callback(data))
});

console.log('Preload loaded');
const { app, BrowserWindow, ipcMain, Menu } = require('electron');
const axios = require('axios');

// ============== SINGLETON PATTERN ==============
class VentanaManager {
  static instancia = null;
  constructor() {
    if (VentanaManager.instancia) return VentanaManager.instancia;
    this.ventanas = {};
    VentanaManager.instancia = this;
  }
  crear(nombre, opciones) {
    if (this.ventanas[nombre]) { this.ventanas[nombre].focus(); return; }
    const win = new BrowserWindow({
      width: opciones.width,
      height: opciones.height,
      parent: opciones.parent,
      webPreferences: {
        preload: opciones.preload,
        contextIsolation: true,
        nodeIntegration: false
      }
    });
    win.loadFile(opciones.archivo);
    win.on('closed', () => { delete this.ventanas[nombre]; });
    this.ventanas[nombre] = win;
  }
  // Notificar a todas las ventanas
  notificarTodos(canal, data) {
    Object.values(this.ventanas).forEach(win => {
      if (!win.isDestroyed()) win.webContents.send(canal, data);
    });
  }
}

// ============== OBSERVER PATTERN ==============
class CuentaObserver {
  constructor(ventanaMgr) { 
    this.observadores = []; 
    this.vm = ventanaMgr;
  }
  agregar(obs) { this.observadores.push(obs); }
  notificar(evento, data) { 
    this.observadores.forEach(o => o.onCambio(evento, data));
    // Notificar a ventanas en tiempo real
    this.vm.notificarTodos('actualizacion', { evento, data });
  }
}

class ConsoleLogger {
  onCambio(evento, data) { }
}

class WebSocketNotifier {
  constructor() { this.sockets = []; }
  onCambio(evento, data) { }
}

// ============== BUILDER PATTERN ==============
class CuentaBuilder {
  constructor() { this.cuenta = {}; }
  setNumero(n) { this.cuenta.numeroCuenta = n; return this; }
  setTitular(t) { this.cuenta.titular = t; return this; }
  setSaldo(s) { this.cuenta.saldo = s; return this; }
  setTasa(t) { this.cuenta.tasaInteres = t; return this; }
  setEstado(e) { this.cuenta.estado = e; return this; }
  build() { return { ...this.cuenta, fechaApertura: new Date().toISOString() }; }
}

// ============== INTERFACE SEGREGATION ==============
class IApiService {
  async request() { throw new Error('Abstract'); }
}

class ICuentaRepository {
  crear() { throw new Error('Abstract'); }
  buscar() { throw new Error('Abstract'); }
  listar() { throw new Error('Abstract'); }
  actualizar() { throw new Error('Abstract'); }
  eliminar() { throw new Error('Abstract'); }
}

class IMovimientoRepository {
  agregar() { throw new Error('Abstract'); }
  listar() { throw new Error('Abstract'); }
}

// ============== DEPENDENCY INVERSION ==============
class ApiService extends IApiService {
  constructor(baseUrl) {
    super();
    this.baseUrl = baseUrl;
  }
  async request(method, endpoint, data = null) {
    try {
      const config = { method: method.toLowerCase(), url: this.baseUrl + endpoint };
      if (data) config.data = data;
      const response = await axios(config);
      return { success: true, data: response.data };
    } catch (e) { 
      return { success: false, error: e.response?.data?.message || e.message };
    }
  }
  listar() { return this.request('GET', ''); }
  crear(cuenta) { return this.request('POST', '', cuenta); }
  buscar(numero) { return this.request('GET', '/' + numero); }
  buscarPorTitular(titular) { return this.request('GET', '/buscar?titular=' + titular); }
  filtrar(titular, estado) {
    let q = '';
    if (titular) q = 'titular=' + titular;
    if (estado && estado !== 'Todos') q += (q ? '&' : '') + 'estado=' + estado;
    return this.request('GET', '/filtrar' + (q ? '?' + q : ''));
  }
  actualizar(numero, cuenta) { return this.request('PUT', '/' + numero, cuenta); }
  eliminar(numero) { return this.request('DELETE', '/' + numero); }
  agregarMovimiento(numero, datos) { return this.request('POST', '/' + numero + '/movimientos', datos); }
  listarMovimientos(numero) { return this.request('GET', '/' + numero + '/movimientos'); }
  listarTodosMovimientos() {return this.request('GET', '/movimientos');
}
}

// ============== SINGLE RESPONSIBILITY ==============
class CuentaRepository extends ICuentaRepository {
  constructor(api) { super(); this.api = api; }
  crear(cuenta) { return this.api.crear(cuenta); }
  listar() { return this.api.listar(); }
  filtrar(titular, estado) { return this.api.filtrar(titular, estado); }
  buscar(numero) { return this.api.buscar(numero); }
  buscarPorTitular(t) { return this.api.buscarPorTitular(t); }
  actualizar(n, c) { return this.api.actualizar(n, c); }
  eliminar(n) { return this.api.eliminar(n); }
}

class MovimientoRepository extends IMovimientoRepository {

  constructor(api) {
    super();
    this.api = api;
  }

  agregar(n, monto, tipo) {
    return this.api.agregarMovimiento(
      n,
      { monto, tipo }
    );
  }

  listar(n) {
    return this.api.listarMovimientos(n);
  }

  listarTodos() {
    return this.api.listarTodosMovimientos();
  }
}

// ============== OPEN/CLOSED ==============
class CuentaService {
  constructor(repo, observer) { this.repo = repo; this.observer = observer; }
  crear(cuenta) {
    const result = this.repo.crear(cuenta);
    this.observer.notificar('CUENTA_CREADA', cuenta);
    return result;
  }
  buscar(n) { return this.repo.buscar(n); }
  listar() { return this.repo.listar(); }
  filtrar(t, es) { return this.repo.filtrar(t, es); }
  actualizar(n, c) {
    const result = this.repo.actualizar(n, c);
    this.observer.notificar('CUENTA_ACTUALIZADA', c);
    return result;
  }
  eliminar(n) {
    const result = this.repo.eliminar(n);
    this.observer.notificar('CUENTA_ELIMINADA', n);
    return result;
  }
}

class MovimientoService {

  constructor(repo, observer) {
    this.repo = repo;
    this.observer = observer;
  }

  agregar(n, monto, tipo) {

    const result =
      this.repo.agregar(n, monto, tipo);

    this.observer.notificar(
      'MOVIMIENTO_AGREGADO',
      { numero: n, monto, tipo }
    );

    return result;
  }

  listar(n) {
    return this.repo.listar(n);
  }

  listarTodos() {
    return this.repo.listarTodos();
  }
}

// ============== LISKOV SUBSTITUTION ==============
class CacheCuentaRepository extends CuentaRepository {
  constructor(api) { super(api); this.cache = new Map(); }
  buscar(numero) {
    if (this.cache.has(numero)) return Promise.resolve({ success: true, data: this.cache.get(numero) });
    const result = super.buscar(numero);
    result.then(r => { if (r.success) this.cache.set(numero, r.data); });
    return result;
  }
}

// Configuración
const API_URL = 'http://localhost:8080/cuentas';
const api = new ApiService(API_URL);
const vm = new VentanaManager();
const repoCuentas = new CuentaRepository(api);
const repoMov = new MovimientoRepository(api);
const observer = new CuentaObserver(vm);
observer.agregar(new ConsoleLogger());
observer.agregar(new WebSocketNotifier());

const cuentaSvc = new CuentaService(repoCuentas, observer);
const movSvc = new MovimientoService(repoMov, observer);

// ============== EXPORT ==============
module.exports = { 
  CuentaBuilder, CuentaRepository, MovimientoRepository,
  CuentaService, MovimientoService, ApiService, CuentaObserver 
};

let principal = null;

function iniciar() {
  principal = new BrowserWindow({ width: 380, height: 220, resizable: false, title: 'WayBank - Electron' });
  principal.loadFile('ventanas/inicio.html');
  
  const menu = Menu.buildFromTemplate([
    { label: 'Crear', click: () => vm.crear('crear', { width: 350, height: 400, archivo: 'ventanas/crear.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
    { label: 'Buscar', click: () => vm.crear('buscar', { width: 350, height: 250, archivo: 'ventanas/buscar.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
    { label: 'Actualizar', click: () => vm.crear('actualizar', { width: 350, height: 300, archivo: 'ventanas/actualizar.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
    { label: 'Eliminar', click: () => vm.crear('eliminar', { width: 350, height: 320, archivo: 'ventanas/eliminar.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
    { label: 'Listar', click: () => vm.crear('listar', { width: 520, height: 380, archivo: 'ventanas/listar.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
    { type: 'separator' },
    { label: 'Movimientos', submenu: [
      { label: 'Agregar', click: () => vm.crear('movimientos', { width: 350, height: 320, archivo: 'ventanas/movimientos.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })},
      { label: 'Ver Historial', click: () => vm.crear('vermovi', { width: 420, height: 340, archivo: 'ventanas/vermovi.html', parent: principal, preload: require('path').join(__dirname, 'preload.js') })}
    ]},
    { type: 'separator' },
    { label: 'Acerca de...', click: () => {
      const { dialog } = require('electron');
      dialog.showMessageBox(principal, { title: 'Acerca de', message: 'WayBank\nCarlos Gil\nJaider Clavijo\nSantiago Lozano\nv1.0' });
    }},
    { label: 'Salir', click: () => app.quit() }
  ]);
  Menu.setApplicationMenu(menu);
}

ipcMain.handle('healthcheck', () => cuentaSvc.listar());
ipcMain.handle('crear-cuenta', (e, c) => cuentaSvc.crear(c));
ipcMain.handle('listar-cuentas', () => cuentaSvc.listar());
ipcMain.handle('filtrar-cuentas', (e, t, es) => cuentaSvc.filtrar(t, es));
ipcMain.handle('buscar-cuenta', (e, n) => cuentaSvc.buscar(n));
ipcMain.handle('buscar-por-titular', (e, t) => repoCuentas.buscarPorTitular(t));
ipcMain.handle('actualizar-cuenta', (e, n, c) => cuentaSvc.actualizar(n, c));
ipcMain.handle('eliminar-cuenta', (e, n) => cuentaSvc.eliminar(n));
ipcMain.handle('agregar-movimiento', (e, n, m, t) => movSvc.agregar(n, m, t));
ipcMain.handle('listar-movimientos', (e, n) => movSvc.listar(n));
ipcMain.handle( 'listar-todos-movimientos',  () => movSvc.listarTodos());

app.whenReady().then(iniciar);
app.on('window-all-closed', () => { if (process.platform !== 'darwin') app.quit(); });
app.on('activate', () => { if (!principal) iniciar(); });
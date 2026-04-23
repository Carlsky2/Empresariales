using RestSharp;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using WayBankClient.model;

namespace WayBankClient.service
{
    public class ServicePeticiones : IServicePeticiones
    {
        private static ServicePeticiones instance;
        private readonly RestClient client;
        public event Action OnCuentasActualizadas;
        public event Action OnMovimientosActualizados;
        private System.Threading.Timer pollingTimer;
        
        
        private ServicePeticiones()
        {
            var options = new RestClientOptions("http://localhost:8080/cuentas");
            client = new RestClient(options);
            // Start a polling timer that notifies subscribers every 2 seconds.
            // Use a 2 second due time so subscribers have a short window to subscribe after creation.
            pollingTimer = new System.Threading.Timer(PollingCallback, null, 2000, 2000);
        }

        private void PollingCallback(object state)
        {
            try
            {
                NotificarCambios();
            }
            catch
            {
                // Swallow exceptions to keep the timer running.
            }
        }

        public static ServicePeticiones GetInstance()
        {
            if (instance == null)
            {
                instance = new ServicePeticiones();
            }

            return instance;
        }


        public bool CrearCuenta(CuentaAhorrosDto cuenta)
        {
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(cuenta);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("Error al crear cuenta: " + response.Content);
                return false;
            }

            return true;
        }

        public List<CuentaAhorrosDto> ListarCuentas()
        {
            var request = new RestRequest("", Method.Get);
            var response = client.Get<List<CuentaAhorrosDto>>(request);
            return response ?? new List<CuentaAhorrosDto>();
        }

        public List<CuentaAhorrosDto> ListarCuentasPorEstado(string estado)
        {
            var request = new RestRequest("filtrar", Method.Get);
            request.AddQueryParameter("estado", estado);
            var response = client.Get<List<CuentaAhorrosDto>>(request);
            return response ?? new List<CuentaAhorrosDto>();
        }

        public List<CuentaAhorrosDto> FiltrarCuentas(string titular, string estado)
        {
            var request = new RestRequest("filtrar", Method.Get);

            if (!string.IsNullOrWhiteSpace(titular))
                request.AddQueryParameter("titular", titular);

            if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
                request.AddQueryParameter("estado", estado);

            var response = client.Get<List<CuentaAhorrosDto>>(request);
            return response ?? new List<CuentaAhorrosDto>();
        }

        public List<CuentaAhorrosDto> BuscarPorTitular(string nombreTitular)
        {
            var request = new RestRequest("buscar", Method.Get);
            request.AddQueryParameter("titular", nombreTitular);
            var response = client.Get<List<CuentaAhorrosDto>>(request);
            return response ?? new List<CuentaAhorrosDto>();
        }

        public CuentaAhorrosDto BuscarPorNumeroCuenta(int numeroCuenta)
        {
            var request = new RestRequest($"{numeroCuenta}", Method.Get);
            var response = client.Execute<CuentaAhorrosDto>(request);

            if (!response.IsSuccessful || response.Data == null)
                return null;

            return response.Data;
        }

        public bool EliminarLogico(int numeroCuenta)
        {
            var request = new RestRequest($"{numeroCuenta}", Method.Delete);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("Error al eliminar cuenta: " + response.Content);
                return false;
            }

            return true;
        }

        public bool ActualizarCuenta(int numeroCuenta, CuentaAhorrosDto cuentaEditada)
        {
            var request = new RestRequest($"{numeroCuenta}", Method.Put);
            request.AddJsonBody(cuentaEditada);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                MessageBox.Show("Error al actualizar cuenta: " + response.Content);
                return false;
            }

            return true;
        }

        public void Healthcheck()
        {
            var request = new RestRequest("healthcheck", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                MessageBox.Show(response.Content, "Estado del servidor",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool AgregarMovimiento(int numeroCuenta, double monto, string tipo)
        {
            var request = new RestRequest($"/{numeroCuenta}/movimientos", Method.Post);
            var datos = new { monto = monto, tipo = tipo.ToUpper() };
            request.AddJsonBody(datos);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Error al agregar movimiento: " + response.Content);
                return false;
            }
        }

        public List<MovimientoDto> ListarMovimientos(int numeroCuenta)
        {
            var request = new RestRequest($"/{numeroCuenta}/movimientos", Method.Get);
            var response = client.Get<List<MovimientoDto>>(request);
            return response ?? new List<MovimientoDto>();
        }

        
        public void NotificarCambios()
        {
            OnCuentasActualizadas?.Invoke();
            OnMovimientosActualizados?.Invoke();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using WayBankClient.model;

namespace WayBankClient.service
{
    public class MovimientoService : IMovimientoService
    {
        private static MovimientoService instance;

        private readonly RestClient client;

        
        private MovimientoService()
        {
            var options = new RestClientOptions(
                "http://localhost:8080/cuentas"
            );

            client = new RestClient(options);
        }

        public static MovimientoService GetInstance()
        {
            if (instance == null)
            {
                instance = new MovimientoService();
            }

            return instance;
        }

        public bool AgregarMovimiento(int numeroCuenta, double monto, string tipo)
        {
            var request = new RestRequest($"/{numeroCuenta}/movimientos", Method.Post);
            var datos = new { monto = monto, tipo = tipo.ToUpper() };
            request.AddJsonBody(datos);
            var response = client.Execute(request);
            return response.IsSuccessful;
        }

        public List<MovimientoDto> ListarMovimientos(int numeroCuenta)
        {
            var request = new RestRequest($"/{numeroCuenta}/movimientos", Method.Get);
            var response = client.Get<List<MovimientoDto>>(request);
            return response ?? new List<MovimientoDto>();
        }

        public List<MovimientoDto> ListarMovimientosPorTipo(int numeroCuenta, string tipo)
        {
            var request = new RestRequest($"/{numeroCuenta}/movimientos/filtrar", Method.Get);
            request.AddQueryParameter("tipo", tipo);
            var response = client.Get<List<MovimientoDto>>(request);
            return response ?? new List<MovimientoDto>();
        }
    }
}

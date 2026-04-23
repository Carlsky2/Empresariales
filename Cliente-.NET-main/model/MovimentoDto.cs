using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WayBankClient.model
{
    public class MovimientoDto
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public string Tipo { get; set; }
        public int NumeroCuenta { get; set; }

        [JsonProperty("fechaMovimiento")]
        public string FechaMovimientoStr { get; set; }

        public DateTime FechaMovimiento
        {
            get
            {
                if (string.IsNullOrEmpty(FechaMovimientoStr))
                    return DateTime.Now;
                return DateTime.Parse(FechaMovimientoStr);
            }
        }

        public MovimientoDto()
        {
            Tipo = "CREDITO";
        }

        public override string ToString()
        {
            return $"Movimiento(Id={Id}, Monto={Monto}, Tipo={Tipo}, Cuenta={NumeroCuenta})";
        }
    }
}

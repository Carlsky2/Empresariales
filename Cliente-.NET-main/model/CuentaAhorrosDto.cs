using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WayBankClient.model
{
    public class CuentaAhorrosDto
    {
        public int NumeroCuenta { get; set; }
        public string Titular { get; set; }
        public double Saldo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaApertura { get; set; }
        public double TasaInteres { get; set; }
    }
}

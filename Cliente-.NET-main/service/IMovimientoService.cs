using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WayBankClient.model;

namespace WayBankClient.service
{
    internal interface IMovimientoService
    {
        bool AgregarMovimiento(int numeroCuenta, double monto,string tipo);

        List<MovimientoDto> ListarMovimientos(int numeroCuenta);

        List<MovimientoDto> ListarMovimientosPorTipo(int numeroCuenta,string tipo);
    }
}

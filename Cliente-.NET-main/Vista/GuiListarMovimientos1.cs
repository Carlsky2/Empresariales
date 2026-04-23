using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WayBankClient.model;
using WayBankClient.service;

namespace WayBankClient.Vista
{
    public partial class GuiListarMovimientos1 : Form
    {
        private ServicePeticiones servicio;
        private CuentaAhorrosDto cuentaActual;
        public GuiListarMovimientos1()
        {
            InitializeComponent();
            servicio = ServicePeticiones.GetInstance();
            servicio.OnMovimientosActualizados += RecargarMovimientos;

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                MessageBox.Show("Ingrese el número de cuenta.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtNumero.Text.Trim(), out int numero))
            {
                MessageBox.Show("Número inválido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cuentaActual = servicio.BuscarPorNumeroCuenta(numero);
            if (cuentaActual == null)
            {
                MessageBox.Show("Cuenta no encontrada.", "Sin resultados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                return;
            }

            txtTitular.Text = cuentaActual.Titular;
            txtSaldo.Text = cuentaActual.Saldo.ToString("N2");

            List<MovimientoDto> movimientos = servicio.ListarMovimientos(numero);
            dgvMovimientos.Rows.Clear();

            if (movimientos.Count == 0)
            {
                lblStatus.Text = "No hay movimientos registrados";
                return;
            }

            foreach (var m in movimientos)
            {
                dgvMovimientos.Rows.Add(
                    m.Id,
                    m.FechaMovimiento.ToString("yyyy-MM-dd HH:mm"),
                    m.Monto.ToString("N2"),
                    m.Tipo
                );
            }

            lblStatus.Text = $"{movimientos.Count} movimiento(s) encontrado(s)";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Limpiar()
        {
            txtTitular.Clear();
            txtSaldo.Clear();
            dgvMovimientos.Rows.Clear();
            lblStatus.Text = "";
            cuentaActual = null;
        }

        private void RecargarMovimientos()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RecargarMovimientos));
                return;
            }

           
            if (cuentaActual != null)
            {
                List<MovimientoDto> movimientos =
                    servicio.ListarMovimientos(
                        cuentaActual.NumeroCuenta
                    );

                dgvMovimientos.Rows.Clear();

                foreach (var m in movimientos)
                {
                    dgvMovimientos.Rows.Add(
                        m.Id,
                        m.FechaMovimiento.ToString("yyyy-MM-dd HH:mm"),
                        m.Monto.ToString("N2"),
                        m.Tipo
                    );
                }

                lblStatus.Text =
                    $"{movimientos.Count} movimiento(s)";
            }
        }

        protected override void OnFormClosed(
    FormClosedEventArgs e)
        {
            servicio.OnMovimientosActualizados -= RecargarMovimientos;

            base.OnFormClosed(e);
        }
    }
}

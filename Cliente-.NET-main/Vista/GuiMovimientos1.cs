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
    public partial class GuiMovimientos1 : Form
    {
        private ServicePeticiones servicio;
        private CuentaAhorrosDto cuentaActual;

        public GuiMovimientos1()
        {
            InitializeComponent();
            servicio = ServicePeticiones.GetInstance();
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
                LimpiarCampos();
                return;
            }

            txtTitular.Text = cuentaActual.Titular;
            txtSaldo.Text = cuentaActual.Saldo.ToString("N2");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cuentaActual == null)
            {
                MessageBox.Show("Busque una cuenta primero.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtMonto.Text.Trim(), out double monto) || monto <= 0)
            {
                MessageBox.Show("Monto inválido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tipo = cmbTipo.SelectedItem.ToString();
            bool ok = servicio.AgregarMovimiento(cuentaActual.NumeroCuenta, monto, tipo);

            if (ok)
            {
                MessageBox.Show("Movimiento agregado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnBuscar_Click(sender, e);
                servicio.NotificarCambios();
                
                txtMonto.Clear();
            }
            else
            {
                MessageBox.Show("Error al agregar movimiento.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtNumero.Clear();
            txtTitular.Clear();
            txtSaldo.Clear();
            txtMonto.Clear();
            
            cuentaActual = null;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

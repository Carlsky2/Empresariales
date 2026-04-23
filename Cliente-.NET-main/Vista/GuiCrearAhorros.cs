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

namespace WayBankClient
{
    public partial class GuiCrearAhorros : Form
    {
        private IServicePeticiones service;
        public GuiCrearAhorros()
        {
            InitializeComponent();
            service = ServicePeticiones.GetInstance();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void GuiCrearAhorros_Load(object sender, EventArgs e)
        {
            pickerTiempo.Format = DateTimePickerFormat.Short;
            pickerTiempo.Value = DateTime.Now;
        }

        private void txtNumCuenta_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSaldo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTitular_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(txtNumCuenta.Text) ||
                    string.IsNullOrWhiteSpace(txtTasaInteres.Text) ||
                    string.IsNullOrWhiteSpace(txtSaldo.Text) ||
                    string.IsNullOrWhiteSpace(txtTitular.Text))
                {
                    MessageBox.Show("Complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int numeroCuenta = int.Parse(txtNumCuenta.Text);
                string titular = txtTitular.Text.Trim();
                double saldo = double.Parse(txtSaldo.Text);
                double tasaInteres = double.Parse(txtTasaInteres.Text);

                
                if (numeroCuenta < 0)
                {
                    MessageBox.Show("El número de cuenta no puede ser negativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNumCuenta.Focus();
                    return;
                }

                
                if (string.IsNullOrEmpty(titular))
                {
                    MessageBox.Show("El titular no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTitular.Focus();
                    return;
                }

                
                if (saldo < 200000.0)
                {
                    MessageBox.Show("El saldo inicial mínimo es $200.000", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaldo.Focus();
                    return;
                }

                
                CuentaAhorrosDto cuenta = new CuentaAhorrosDto
                {
                    NumeroCuenta = numeroCuenta,
                    Titular = titular,
                    TasaInteres = tasaInteres,
                    Saldo = saldo,
                    Estado = "Activo",
                    FechaApertura = new DateTime(pickerTiempo.Value.Year, pickerTiempo.Value.Month, pickerTiempo.Value.Day, pickerTiempo.Value.Hour, pickerTiempo.Value.Minute, pickerTiempo.Value.Second, DateTimeKind.Unspecified)
                };

                bool creado = service.CrearCuenta(cuenta);

                if (creado)
                {
                    service.NotificarCambios();
                    MessageBox.Show("Cuenta creada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo crear la cuenta (Error en el servidor).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Verifique que Número de cuenta, Tasa de interés y Saldo sean numéricos.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pickerTiempo_ValueChanged(object sender, EventArgs e)
        {

        }
        private void LimpiarCampos()
        {
            txtNumCuenta.Clear();
            txtTasaInteres.Clear();
            txtSaldo.Clear();
            txtTitular.Clear();

            pickerTiempo.Value = DateTime.Now;
            txtNumCuenta.Focus();
        }
    }
}


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
    public partial class GuiBuscarAhorros : Form
    {
        private IServicePeticiones service;
        public GuiBuscarAhorros()
        {
            InitializeComponent();
            service = ServicePeticiones.GetInstance();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbxFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string criterio = cmbxFiltrar.Text;
                string valor = txtParametro.Text.Trim();

                if (string.IsNullOrWhiteSpace(valor))
                {
                    MessageBox.Show("Ingrese un valor para buscar.");
                    return;
                }

                if (criterio == "NumeroCuenta")
                {
                    if (!int.TryParse(valor, out int numero))
                    {
                        MessageBox.Show("Ingrese un número válido.");
                        return;
                    }

                    var cuenta = service.BuscarPorNumeroCuenta(numero);

                    if (cuenta == null)
                    {
                        MessageBox.Show("Cuenta no encontrada.");
                        return;
                    }

                    if (cuenta.Estado != "Activo")
                    {
                        MessageBox.Show("La cuenta se encuentra inactiva.", "Cuenta inactiva",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MostrarCuenta(cuenta);
                }
                else if (criterio == "Titular")
                {
                    var lista = service.BuscarPorTitular(valor);

                    if (lista.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                        return;
                    }

                    
                    if (lista[0].Estado != "Activo")
                    {
                        MessageBox.Show("La cuenta se encuentra inactiva.", "Cuenta inactiva",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MostrarCuenta(lista[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        

        private void GuiBuscarAhorros_Load(object sender, EventArgs e)
        {
            cmbxFiltrar.Items.Clear();
            cmbxFiltrar.Items.Add("NumeroCuenta");
            cmbxFiltrar.Items.Add("Titular");
            cmbxFiltrar.SelectedIndex = 0;

        }

        private void txtParametro_TextChanged(object sender, EventArgs e)
        {

        }
        private void MostrarCuenta(CuentaAhorrosDto cuenta)
        {
            txtNumCuenta.Text = cuenta.NumeroCuenta.ToString();
            txtTitular.Text = cuenta.Titular;
            txtSaldo.Text = cuenta.Saldo.ToString();
            txtTasaInteres.Text = cuenta.TasaInteres.ToString();
            
            txtFecha.Text = cuenta.FechaApertura.ToString("yyyy-MM-dd");
        }
    }
}

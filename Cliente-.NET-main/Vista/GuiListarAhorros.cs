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
    public partial class GuiListarAhorros : Form
    {
        private IServicePeticiones service;
        public GuiListarAhorros()
        {
            InitializeComponent();
            service = ServicePeticiones.GetInstance();
            ((ServicePeticiones)service).OnCuentasActualizadas += RecargarTabla;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ((ServicePeticiones)service).OnCuentasActualizadas -= RecargarTabla;
            base.OnFormClosed(e);
        }

        private void RecargarTabla()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RecargarTabla));
                return;
            }

            CargarCuentas();
        }

        private void GuiListarAhorros_Load(object sender, EventArgs e)
        {
            cmbxFiltrar.Items.Clear();
            cmbxFiltrar.Items.Add("Todos");
            cmbxFiltrar.Items.Add("Activo");
            cmbxFiltrar.Items.Add("Inactivo");
            cmbxFiltrar.SelectedIndex = 0;

            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;

            CargarCuentas();
        }

        private void CargarCuentas()
        {
            var lista = service.ListarCuentas();
            ActualizarTabla(lista);
        }

        private void ActualizarTabla(List<CuentaAhorrosDto> lista)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            string titular = txtTitular.Text.Trim();
            string estado = cmbxFiltrar.Text;

            
            var lista = service.FiltrarCuentas(titular, estado);

            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WayBankClient.Vista
{
    public partial class GuiInicio : Form
    {
        public GuiInicio()
        {
            InitializeComponent();
        }

        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiCrearAhorros crearAhorros = new GuiCrearAhorros();
            crearAhorros.Show();
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiBuscarAhorros buscarAhorros = new GuiBuscarAhorros();
            buscarAhorros.Show();
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiActualizarAhorros actualizarAhorros = new GuiActualizarAhorros(); 
            actualizarAhorros.Show();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiEliminarAhorros eliminarAhorros = new GuiEliminarAhorros();
            eliminarAhorros.Show();
        }

        private void listarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiListarAhorros listarAhorros = new GuiListarAhorros(); 
            listarAhorros.Show();
        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiMovimientos1 guiMovimientos1 = new GuiMovimientos1();
            guiMovimientos1.Show();
        }

        private void listarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GuiListarMovimientos1 gui = new GuiListarMovimientos1(); 
            gui.Show();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Desarrollado por Carlos, Jaider y Santiago!!! Ver 0.0.2 ", "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

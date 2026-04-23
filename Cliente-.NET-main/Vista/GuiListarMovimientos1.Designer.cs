using System;
using System.Drawing;
using System.Windows.Forms;

namespace WayBankClient.Vista
{
    partial class GuiListarMovimientos1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumero = new TextBox();
            this.txtTitular = new TextBox();
            this.txtSaldo = new TextBox();
            this.dgvMovimientos = new DataGridView();
            this.lblStatus = new Label();
            this.btnBuscar = new Button();
            this.btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNumero
            // 
            this.txtNumero.Location = new Point(120, 50);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new Size(120, 20);
            this.txtNumero.TabIndex = 0;
            // 
            // Label Número
            // 
            Label lblNumero = new Label();
            lblNumero.AutoSize = true;
            lblNumero.Location = new Point(12, 53);
            lblNumero.Name = "lblNumero";
            lblNumero.Size = new Size(95, 13);
            lblNumero.Text = "Número cuenta:";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new Point(260, 48);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new Size(90, 23);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // Label Titular
            // 
            Label lblTitular = new Label();
            lblTitular.AutoSize = true;
            lblTitular.Location = new Point(12, 90);
            lblTitular.Name = "lblTitular";
            lblTitular.Size = new Size(40, 13);
            lblTitular.Text = "Titular:";
            // 
            // txtTitular
            // 
            this.txtTitular.Location = new Point(120, 87);
            this.txtTitular.Name = "txtTitular";
            this.txtTitular.ReadOnly = true;
            this.txtTitular.Size = new Size(230, 20);
            this.txtTitular.TabIndex = 2;
            // 
            // Label Saldo
            // 
            Label lblSaldo = new Label();
            lblSaldo.AutoSize = true;
            lblSaldo.Location = new Point(12, 120);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Size = new Size(70, 13);
            lblSaldo.Text = "Saldo actual:";
            // 
            // txtSaldo
            // 
            this.txtSaldo.Location = new Point(120, 117);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.ReadOnly = true;
            this.txtSaldo.Size = new Size(150, 20);
            this.txtSaldo.TabIndex = 3;
            // 
            // Label Historial
            // 
            Label lblHistorial = new Label();
            lblHistorial.AutoSize = true;
            lblHistorial.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblHistorial.Location = new Point(12, 155);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(140, 15);
            lblHistorial.Text = "Historial de movimientos:";
            // 
            // dgvMovimientos
            // 
            this.dgvMovimientos.AllowUserToAddRows = false;
            this.dgvMovimientos.AllowUserToDeleteRows = false;
            this.dgvMovimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientos.Location = new Point(12, 175);
            this.dgvMovimientos.Name = "dgvMovimientos";
            this.dgvMovimientos.ReadOnly = true;
            this.dgvMovimientos.RowTemplate.Height = 21;
            this.dgvMovimientos.Size = new Size(340, 180);
            this.dgvMovimientos.TabIndex = 4;
            this.dgvMovimientos.Columns.Add("Id", "ID");
            this.dgvMovimientos.Columns.Add("Fecha", "Fecha");
            this.dgvMovimientos.Columns.Add("Monto", "Monto");
            this.dgvMovimientos.Columns.Add("Tipo", "Tipo");
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(12, 365);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(0, 13);
            this.lblStatus.TabIndex = 5;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new Point(272, 390);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new Size(80, 25);
            this.btnCerrar.TabIndex = 6;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // GuiListarMovimientos
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(364, 428);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(lblHistorial);
            this.Controls.Add(this.dgvMovimientos);
            this.Controls.Add(this.txtSaldo);
            this.Controls.Add(lblSaldo);
            this.Controls.Add(this.txtTitular);
            this.Controls.Add(lblTitular);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtNumero);
            this.Controls.Add(lblNumero);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GuiListarMovimientos";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Ver Movimientos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            
    }

        #endregion

        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.TextBox txtTitular;
        private System.Windows.Forms.TextBox txtSaldo;

        private System.Windows.Forms.DataGridView dgvMovimientos;

        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.Label lblTitular;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblHistorial;

        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnCerrar;

        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMonto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
    }
}
namespace BankIA.Forms
{
    partial class FrmTransferencia
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.TextBox txtCuentaDestino = new();
        internal System.Windows.Forms.TextBox txtMonto         = new();
        internal System.Windows.Forms.Label   lblTitular       = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Transferencia", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };

            var lblCuenta = new System.Windows.Forms.Label { Text = "Cuenta destino:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 72) };
            txtCuentaDestino.Location = new System.Drawing.Point(24, 94); txtCuentaDestino.Size = new System.Drawing.Size(320, 34);
            txtCuentaDestino.Font = DisGen.Orbitron(11f); txtCuentaDestino.MaxLength = 20;
            txtCuentaDestino.BackColor = DisGen.CSuperficie; txtCuentaDestino.ForeColor = DisGen.CBlanco; txtCuentaDestino.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            var lblTitL = new System.Windows.Forms.Label { Text = "Titular:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 152) };
            lblTitular.AutoSize = true; lblTitular.Font = DisGen.Exo2(11f, System.Drawing.FontStyle.Bold); lblTitular.ForeColor = DisGen.CBlanco; lblTitular.Location = new System.Drawing.Point(24, 172); lblTitular.Text = "—";

            var lblMontoL = new System.Windows.Forms.Label { Text = "Monto MXN:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 220) };
            txtMonto.Location = new System.Drawing.Point(24, 242); txtMonto.Size = new System.Drawing.Size(240, 34);
            txtMonto.Font = DisGen.Orbitron(13f); txtMonto.BackColor = DisGen.CSuperficie;
            txtMonto.ForeColor = DisGen.COro; txtMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            var btnConf = new System.Windows.Forms.Button { Text = "CONFIRMAR", Location = new System.Drawing.Point(580, 370), Size = new System.Drawing.Size(150, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.COro, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            DisGen.EstilarBtnPrimario(btnConf, DisGen.COro);
            btnConf.Click += BtnConfirmar_Click;

            this.SuspendLayout();
            this.Name = "FrmTransferencia"; this.Text = "BankIA — Transferencia";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitulo, lblCuenta, txtCuentaDestino, lblTitL, lblTitular, lblMontoL, txtMonto, btnConf });
            this.ResumeLayout(false);
        }
    }
}

namespace BankIA.Forms
{
    partial class FrmSaldo
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Label lblSaldoDisponible = new();
        internal System.Windows.Forms.Label lblSaldoContable   = new();
        internal System.Windows.Forms.Label lblNumeroCuenta    = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Su Saldo", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };

            var lblDispL = new System.Windows.Forms.Label { Text = "Saldo disponible", AutoSize = true, Font = DisGen.Exo2(8f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 60) };
            lblSaldoDisponible.AutoSize = true; lblSaldoDisponible.Font = DisGen.Orbitron(34f, System.Drawing.FontStyle.Bold); lblSaldoDisponible.ForeColor = DisGen.CCyan; lblSaldoDisponible.Location = new System.Drawing.Point(24, 78); lblSaldoDisponible.Text = "$0.00";
            lblNumeroCuenta.AutoSize = true; lblNumeroCuenta.Font = DisGen.Exo2(9f); lblNumeroCuenta.ForeColor = DisGen.CMuted; lblNumeroCuenta.Location = new System.Drawing.Point(24, 152); lblNumeroCuenta.Text = "Cuenta ••••";

            var panelStats = new System.Windows.Forms.Panel { Location = new System.Drawing.Point(24, 175), Size = new System.Drawing.Size(880, 80), BackColor = DisGen.CPanel, BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle };
            var lblContL = new System.Windows.Forms.Label { Text = "Saldo contable", AutoSize = true, Font = DisGen.Exo2(8f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(20, 10) };
            lblSaldoContable.AutoSize = true; lblSaldoContable.Font = DisGen.Orbitron(14f); lblSaldoContable.ForeColor = DisGen.COro; lblSaldoContable.Location = new System.Drawing.Point(20, 30); lblSaldoContable.Text = "$0.00";
            panelStats.Controls.AddRange(new System.Windows.Forms.Control[] { lblContL, lblSaldoContable });

            var btnMovs = new System.Windows.Forms.Button { Text = "VER MOVIMIENTOS", Location = new System.Drawing.Point(24, 330), Size = new System.Drawing.Size(220, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.CCyan, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            btnMovs.FlatAppearance.BorderColor = DisGen.CCyanDim; btnMovs.Click += BtnMovimientos_Click;

            this.SuspendLayout();
            this.Name = "FrmSaldo"; this.Text = "BankIA — Consulta de Saldo";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitulo, lblDispL, lblSaldoDisponible, lblNumeroCuenta, panelStats, btnMovs });
            this.ResumeLayout(false);
        }
    }
}

namespace BankIA.Forms
{
    partial class FrmDeposito
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.TextBox txtMonto = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Depósito de Efectivo", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };
            var lblInstr  = new System.Windows.Forms.Label { Text = "Introduzca los billetes en la ranura:", AutoSize = true, Font = DisGen.Exo2(10f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 80) };
            var lblM      = new System.Windows.Forms.Label { Text = "Total a depositar:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 135) };
            txtMonto.Location = new System.Drawing.Point(24, 157); txtMonto.Size = new System.Drawing.Size(240, 34);
            txtMonto.Font = DisGen.Orbitron(13f); txtMonto.BackColor = DisGen.CSuperficie;
            txtMonto.ForeColor = DisGen.CVerde; txtMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            var btnDep = new System.Windows.Forms.Button { Text = "DEPOSITAR", Location = new System.Drawing.Point(580, 370), Size = new System.Drawing.Size(150, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.CVerde, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            DisGen.EstilarBtnPrimario(btnDep, DisGen.CVerde);
            btnDep.Click += BtnDepositar_Click;

            this.SuspendLayout();
            this.Name = "FrmDeposito"; this.Text = "BankIA — Depósito";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitulo, lblInstr, lblM, txtMonto, btnDep });
            this.ResumeLayout(false);
        }
    }
}

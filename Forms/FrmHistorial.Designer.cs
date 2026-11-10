namespace BankIA.Forms
{
    partial class FrmHistorial
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.ListBox lstMovimientos = new();
        internal System.Windows.Forms.Button  btnComprobante = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Últimos movimientos", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };

            lstMovimientos.Location  = new System.Drawing.Point(24, 55);
            lstMovimientos.Size      = new System.Drawing.Size(905, 340);
            lstMovimientos.Font      = DisGen.Orbitron(8.5f);
            lstMovimientos.BackColor = DisGen.CSuperficie;
            lstMovimientos.ForeColor = DisGen.CBlanco;
            lstMovimientos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstMovimientos.SelectionMode = System.Windows.Forms.SelectionMode.One;

            btnComprobante.Text      = "📄 COMPROBANTE";
            btnComprobante.Location  = new System.Drawing.Point(220, 400);
            btnComprobante.Size      = new System.Drawing.Size(170, 40);
            btnComprobante.Font      = DisGen.Orbitron(7.5f);
            btnComprobante.BackColor = DisGen.CPanel;
            btnComprobante.ForeColor = DisGen.CMuted;
            btnComprobante.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnComprobante.FlatAppearance.BorderColor = DisGen.CBorde;
            btnComprobante.Enabled   = false;
            btnComprobante.Cursor    = System.Windows.Forms.Cursors.Hand;
            btnComprobante.Click    += BtnComprobante_Click;

            this.SuspendLayout();
            this.Name = "FrmHistorial"; this.Text = "BankIA — Historial";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblTitulo, lstMovimientos, btnComprobante });
            this.ResumeLayout(false);
        }
    }
}

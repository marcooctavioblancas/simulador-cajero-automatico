namespace BankIA.Forms
{
    partial class FrmCambioNIP
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.TextBox txtNIPActual    = new();
        internal System.Windows.Forms.TextBox txtNIPNuevo     = new();
        internal System.Windows.Forms.TextBox txtNIPConfirmar = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.TextBox CampoNIP(int y)
        {
            return new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(430, y), Size = new System.Drawing.Size(160, 34),
                Font = DisGen.Orbitron(14f), BackColor = DisGen.CSuperficie,
                ForeColor = DisGen.CBlanco, BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                MaxLength = 4, PasswordChar = '●'
            };
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Cambio de NIP", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };

            var lbl1 = new System.Windows.Forms.Label { Text = "1  NIP Actual", AutoSize = true, Font = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CVerde, Location = new System.Drawing.Point(80, 84) };
            txtNIPActual = CampoNIP(80);
            var lbl2 = new System.Windows.Forms.Label { Text = "2  Nuevo NIP", AutoSize = true, Font = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(80, 164) };
            txtNIPNuevo = CampoNIP(160);
            var lbl3 = new System.Windows.Forms.Label { Text = "3  Confirmar NIP", AutoSize = true, Font = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(80, 244) };
            txtNIPConfirmar = CampoNIP(240);

            var btnCont = new System.Windows.Forms.Button { Text = "CONTINUAR", Location = new System.Drawing.Point(580, 370), Size = new System.Drawing.Size(150, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.CCyan, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            btnCont.FlatAppearance.BorderColor = DisGen.CCyanDim; btnCont.Click += BtnContinuar_Click;

            this.SuspendLayout();
            this.Name = "FrmCambioNIP"; this.Text = "BankIA — Cambio de NIP";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitulo, lbl1, txtNIPActual, lbl2, txtNIPNuevo, lbl3, txtNIPConfirmar, btnCont });
            this.ResumeLayout(false);
        }
    }
}

namespace BankIA.Forms
{
    partial class FrmRetiro
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.TextBox txtMonto    = new();
        internal System.Windows.Forms.Label   lblLimite  = new();
        internal System.Windows.Forms.Label   lblDesglose = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button CrearMonto(string texto, string tag, int x, int y)
        {
            var b = new System.Windows.Forms.Button
            {
                Text = texto, Tag = tag, Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(130, 70),
                Font = DisGen.Orbitron(11f, System.Drawing.FontStyle.Bold),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CBlanco,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            b.FlatAppearance.BorderColor = DisGen.CBorde;
            b.Click += BtnMonto_Click;
            return b;
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label { Text = "Retiro de Efectivo", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };

            var bm200  = CrearMonto("$200",   "200",  24,  60);
            var bm500  = CrearMonto("$500",   "500",  174, 60);
            var bm1000 = CrearMonto("$1,000", "1000", 324, 60);
            var bm2000 = CrearMonto("$2,000", "2000", 474, 60);
            var bm3000 = CrearMonto("$3,000", "3000", 624, 60);

            var lblMonto = new System.Windows.Forms.Label { Text = "Monto:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 162) };
            txtMonto.Location = new System.Drawing.Point(24, 184); txtMonto.Size = new System.Drawing.Size(220, 34);
            txtMonto.Font = DisGen.Orbitron(13f); txtMonto.BackColor = DisGen.CSuperficie;
            txtMonto.ForeColor = DisGen.CBlanco; txtMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMonto.TextChanged += TxtMonto_TextChanged;

            lblLimite.AutoSize  = true;
            lblLimite.Font      = DisGen.Exo2(9f);
            lblLimite.ForeColor = DisGen.CMuted;
            lblLimite.Location  = new System.Drawing.Point(24, 232);
            lblLimite.Text      = "Límite diario restante: calculando...";

            lblDesglose.AutoSize    = true;
            lblDesglose.Font        = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold);
            lblDesglose.ForeColor   = DisGen.CVerde;
            lblDesglose.Location    = new System.Drawing.Point(24, 260);
            lblDesglose.MaximumSize = new System.Drawing.Size(860, 0);
            lblDesglose.Text        = string.Empty;

            var btnConfirmar = new System.Windows.Forms.Button { Text = "CONFIRMAR", Location = new System.Drawing.Point(580, 370), Size = new System.Drawing.Size(150, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.COro, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            DisGen.EstilarBtnPrimario(btnConfirmar, DisGen.COro);
            btnConfirmar.Click += BtnConfirmar_Click;

            this.SuspendLayout();
            this.Name = "FrmRetiro"; this.Text = "BankIA — Retiro";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitulo, bm200, bm500, bm1000, bm2000, bm3000, lblMonto, txtMonto, lblLimite, lblDesglose, btnConfirmar });
            this.ResumeLayout(false);
        }
    }
}

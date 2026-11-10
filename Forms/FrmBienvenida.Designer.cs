namespace BankIA.Forms
{
    partial class FrmBienvenida
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel   _pnlCentro  = new();
        private System.Windows.Forms.Label   _lblBank    = new();
        private System.Windows.Forms.Label   _lblIA      = new();
        private System.Windows.Forms.Label   _lblSlogan  = new();
        internal System.Windows.Forms.Label  _lblInsert  = new();
        private System.Windows.Forms.Label   _lblFlecha  = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _pulseTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Logo: "BANK" + "IA" ───────────────────────────────────
            _lblBank.Text      = "BANK";
            _lblBank.Font      = DisGen.Orbitron(38f, System.Drawing.FontStyle.Bold);
            _lblBank.ForeColor = DisGen.CCyan;
            _lblBank.AutoSize  = true;
            _lblBank.Location  = new System.Drawing.Point(0, 0);
            _lblBank.BackColor = System.Drawing.Color.Transparent;

            _lblIA.Text      = "IA";
            _lblIA.Font      = DisGen.Orbitron(38f, System.Drawing.FontStyle.Bold);
            _lblIA.ForeColor = DisGen.COro;
            _lblIA.AutoSize  = true;
            _lblIA.Location  = new System.Drawing.Point(225, 0);
            _lblIA.BackColor = System.Drawing.Color.Transparent;

            // ── Subtítulo ─────────────────────────────────────────────
            _lblSlogan.Text      = "INTELIGENCIA FINANCIERA";
            _lblSlogan.Font      = DisGen.Exo2(9f);
            _lblSlogan.ForeColor = DisGen.CMuted;
            _lblSlogan.AutoSize  = true;
            _lblSlogan.Location  = new System.Drawing.Point(0, 68);
            _lblSlogan.BackColor = System.Drawing.Color.Transparent;

            // ── Texto pulsante ────────────────────────────────────────
            _lblInsert.Text      = "↳  INSERTE SU TARJETA PARA COMENZAR";
            _lblInsert.Font      = DisGen.Orbitron(9.5f);
            _lblInsert.ForeColor = DisGen.CCyan;
            _lblInsert.AutoSize  = true;
            _lblInsert.Location  = new System.Drawing.Point(0, 110);
            _lblInsert.BackColor = System.Drawing.Color.Transparent;
            _lblInsert.Cursor    = System.Windows.Forms.Cursors.Hand;

            // ── Flecha ────────────────────────────────────────────────
            _lblFlecha.Text      = "⬆";
            _lblFlecha.Font      = new System.Drawing.Font("Segoe UI Symbol", 22f);
            _lblFlecha.ForeColor = DisGen.COro;
            _lblFlecha.AutoSize  = true;
            _lblFlecha.Location  = new System.Drawing.Point(0, 148);
            _lblFlecha.BackColor = System.Drawing.Color.Transparent;

            // ── Panel contenedor ──────────────────────────────────────
            _pnlCentro.BackColor = System.Drawing.Color.Transparent;
            _pnlCentro.Size      = new System.Drawing.Size(620, 260);
            _pnlCentro.Controls.AddRange(new System.Windows.Forms.Control[]
                { _lblBank, _lblIA, _lblSlogan, _lblInsert, _lblFlecha });

            // Clic en labels también navega a insertar tarjeta
            foreach (System.Windows.Forms.Control c in _pnlCentro.Controls)
                c.Click += FrmBienvenida_Click;

            // ── Botón SIN TARJETA ──────────────────────────────────────
            var btnSinTarjeta = new System.Windows.Forms.Button
            {
                Text      = "SIN TARJETA",
                Font      = DisGen.Orbitron(7.5f),
                ForeColor = DisGen.CCyan,
                BackColor = System.Drawing.Color.FromArgb(15, 32, 64),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                AutoSize  = true,
                Cursor    = System.Windows.Forms.Cursors.Hand,
                Location  = new System.Drawing.Point(0, 210),
                Padding   = new System.Windows.Forms.Padding(14, 6, 14, 6)
            };
            btnSinTarjeta.FlatAppearance.BorderColor = DisGen.CCyanDim;
            btnSinTarjeta.FlatAppearance.BorderSize  = 1;
            btnSinTarjeta.Click += BtnSinTarjeta_Click;
            _pnlCentro.Controls.Add(btnSinTarjeta);

            this.SuspendLayout();
            this.Name = "FrmBienvenida";
            this.Text = "BankIA — Bienvenida";
            this.PanelContenido.Controls.Add(_pnlCentro);
            this.ResumeLayout(false);
        }
    }
}

namespace BankIA.Forms
{
    partial class FrmRetiroSinTarjeta
    {
        private System.ComponentModel.IContainer components = null;

        // Paso 1 — cuenta
        internal System.Windows.Forms.Panel   pnlPaso1            = new();
        internal System.Windows.Forms.TextBox txtCuenta           = new();
        internal System.Windows.Forms.Label   lblTitularDetectado = new();

        // Paso 2 — NIP
        internal System.Windows.Forms.Panel pnlPaso2  = new();
        internal System.Windows.Forms.Panel pnlDotST1 = new();
        internal System.Windows.Forms.Panel pnlDotST2 = new();
        internal System.Windows.Forms.Panel pnlDotST3 = new();
        internal System.Windows.Forms.Panel pnlDotST4 = new();

        // Paso 3 — monto
        internal System.Windows.Forms.Panel   pnlPaso3    = new();
        internal System.Windows.Forms.TextBox txtMonto    = new();
        internal System.Windows.Forms.Label   lblSaldoDisp = new();
        internal System.Windows.Forms.Label   lblDesgloseST = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button CrearTecla(string texto, string tag, int x, int y)
        {
            var btn = new System.Windows.Forms.Button
            {
                Text = texto, Tag = tag,
                Location = new System.Drawing.Point(x, y), Size = new System.Drawing.Size(70, 52),
                Font = DisGen.Orbitron(13f, System.Drawing.FontStyle.Bold),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CBlanco,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = DisGen.CBorde;
            return btn;
        }

        private System.Windows.Forms.Button CrearBtnMonto(string texto, string tag, int x, int y)
        {
            var b = new System.Windows.Forms.Button
            {
                Text = texto, Tag = tag, Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(120, 60),
                Font = DisGen.Orbitron(10f, System.Drawing.FontStyle.Bold),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CBlanco,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand
            };
            b.FlatAppearance.BorderColor = DisGen.CBorde;
            b.Click += BtnMonto_Click;
            return b;
        }

        private void InitializeComponent()
        {
            // ── Paso 1: cuenta ────────────────────────────────────────────
            pnlPaso1.BackColor = System.Drawing.Color.Transparent;
            pnlPaso1.Location  = new System.Drawing.Point(0, 0);
            pnlPaso1.Size      = new System.Drawing.Size(920, 460);

            var lblTit1 = new System.Windows.Forms.Label { Text = "Retiro Sin Tarjeta", AutoSize = true, Font = DisGen.Orbitron(13f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(24, 14) };
            var lblComL = new System.Windows.Forms.Label { Text = $"Comisión por servicio: ${Config.AppConfig.ComisionSinTarjeta:N2} MXN", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 44) };
            var lblCuentaL = new System.Windows.Forms.Label { Text = "Número de cuenta:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 100) };

            txtCuenta.Location = new System.Drawing.Point(24, 122); txtCuenta.Size = new System.Drawing.Size(340, 34);
            txtCuenta.Font = DisGen.Orbitron(11f); txtCuenta.BackColor = DisGen.CSuperficie;
            txtCuenta.ForeColor = DisGen.CBlanco; txtCuenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCuenta.MaxLength = 20;

            lblTitularDetectado.AutoSize = true; lblTitularDetectado.Font = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold);
            lblTitularDetectado.ForeColor = DisGen.CVerde; lblTitularDetectado.Location = new System.Drawing.Point(24, 168); lblTitularDetectado.Text = string.Empty;

            var btnBuscar = new System.Windows.Forms.Button { Text = "CONTINUAR →", Location = new System.Drawing.Point(380, 122), Size = new System.Drawing.Size(150, 34), Font = DisGen.Orbitron(8f), BackColor = DisGen.CPanel, ForeColor = DisGen.CCyan, FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            btnBuscar.FlatAppearance.BorderColor = DisGen.CCyanDim; btnBuscar.Click += BtnBuscarCuenta_Click;

            pnlPaso1.Controls.AddRange(new System.Windows.Forms.Control[] { lblTit1, lblComL, lblCuentaL, txtCuenta, lblTitularDetectado, btnBuscar });

            // ── Paso 2: NIP ───────────────────────────────────────────────
            pnlPaso2.BackColor = System.Drawing.Color.Transparent;
            pnlPaso2.Location  = new System.Drawing.Point(0, 0);
            pnlPaso2.Size      = new System.Drawing.Size(920, 460);

            var lblNIPL = new System.Windows.Forms.Label { Text = "Ingrese su NIP", AutoSize = true, Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(300, 14) };

            int dotY = 60;
            foreach (var (dot, i) in new[] { pnlDotST1, pnlDotST2, pnlDotST3, pnlDotST4 }.Select((d, i) => (d, i)))
            {
                dot.Size = new System.Drawing.Size(32, 32); dot.Location = new System.Drawing.Point(296 + i * 44, dotY);
                dot.BackColor = System.Drawing.Color.Transparent; dot.BorderStyle = System.Windows.Forms.BorderStyle.None; dot.Tag = false;
                dot.Paint += PintarDot;
            }

            var b1 = CrearTecla("1","1",300,110); b1.Click += BtnNumero_Click;
            var b2 = CrearTecla("2","2",378,110); b2.Click += BtnNumero_Click;
            var b3 = CrearTecla("3","3",456,110); b3.Click += BtnNumero_Click;
            var b4 = CrearTecla("4","4",300,170); b4.Click += BtnNumero_Click;
            var b5 = CrearTecla("5","5",378,170); b5.Click += BtnNumero_Click;
            var b6 = CrearTecla("6","6",456,170); b6.Click += BtnNumero_Click;
            var b7 = CrearTecla("7","7",300,230); b7.Click += BtnNumero_Click;
            var b8 = CrearTecla("8","8",378,230); b8.Click += BtnNumero_Click;
            var b9 = CrearTecla("9","9",456,230); b9.Click += BtnNumero_Click;
            var b0 = CrearTecla("0","0",378,290); b0.Click += BtnNumero_Click;

            var bDel = CrearTecla("⌫","del",300,290); bDel.ForeColor = DisGen.CRojo; bDel.Click += BtnBorrar_Click;
            var bOk  = CrearTecla("✓","ok", 456,290); bOk.ForeColor  = DisGen.CVerde; bOk.Click  += BtnVerificarNIP_Click;

            pnlPaso2.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblNIPL, pnlDotST1, pnlDotST2, pnlDotST3, pnlDotST4,
                b1, b2, b3, b4, b5, b6, b7, b8, b9, b0, bDel, bOk
            });

            // ── Paso 3: monto ─────────────────────────────────────────────
            pnlPaso3.BackColor = System.Drawing.Color.Transparent;
            pnlPaso3.Location  = new System.Drawing.Point(0, 0);
            pnlPaso3.Size      = new System.Drawing.Size(920, 460);

            lblSaldoDisp.AutoSize = true; lblSaldoDisp.Font = DisGen.Exo2(9f); lblSaldoDisp.ForeColor = DisGen.CMuted; lblSaldoDisp.Location = new System.Drawing.Point(24, 14);
            var bm200  = CrearBtnMonto("$200",  "200",  24, 44);
            var bm500  = CrearBtnMonto("$500",  "500", 154, 44);
            var bm1000 = CrearBtnMonto("$1,000","1000",284, 44);
            var bm2000 = CrearBtnMonto("$2,000","2000",414, 44);
            var bm3000 = CrearBtnMonto("$3,000","3000",544, 44);
            var lblM = new System.Windows.Forms.Label { Text = "Otro monto:", AutoSize = true, Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted, Location = new System.Drawing.Point(24, 128) };
            txtMonto.Location = new System.Drawing.Point(24, 148); txtMonto.Size = new System.Drawing.Size(200, 34);
            txtMonto.Font = DisGen.Orbitron(13f); txtMonto.BackColor = DisGen.CSuperficie; txtMonto.ForeColor = DisGen.COro; txtMonto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMonto.TextChanged += TxtMonto_TextChanged;
            lblDesgloseST.AutoSize = true; lblDesgloseST.Font = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold); lblDesgloseST.ForeColor = DisGen.CVerde; lblDesgloseST.Location = new System.Drawing.Point(24, 200); lblDesgloseST.MaximumSize = new System.Drawing.Size(860, 0);
            var btnConf = new System.Windows.Forms.Button { Text = "CONFIRMAR", Location = new System.Drawing.Point(680, 370), Size = new System.Drawing.Size(160, 46), Font = DisGen.Orbitron(8f), BackColor = DisGen.CPanel, ForeColor = DisGen.COro, FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            DisGen.EstilarBtnPrimario(btnConf, DisGen.COro); btnConf.Click += BtnConfirmar_Click;

            pnlPaso3.Controls.AddRange(new System.Windows.Forms.Control[] { lblSaldoDisp, bm200, bm500, bm1000, bm2000, bm3000, lblM, txtMonto, lblDesgloseST, btnConf });

            // ── Form ──────────────────────────────────────────────────────
            this.SuspendLayout();
            this.Name = "FrmRetiroSinTarjeta";
            this.Text = "BankIA — Retiro Sin Tarjeta";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { pnlPaso1, pnlPaso2, pnlPaso3 });
            this.ResumeLayout(false);
        }
    }
}

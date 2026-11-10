namespace BankIA.Forms
{
    partial class FrmMenu
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Label lblBienvenido = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button CrearOpcion(string icono, string label, string sub,
            System.Drawing.Color borde, int x, int y)
        {
            var p = new System.Windows.Forms.Button
            {
                Text      = $"{icono}\n{label}\n{sub}",
                Location  = new System.Drawing.Point(x, y),
                Size      = new System.Drawing.Size(215, 120),
                BackColor = DisGen.CPanel,
                ForeColor = DisGen.CBlanco,
                Font      = DisGen.Orbitron(9f, System.Drawing.FontStyle.Bold),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            p.FlatAppearance.BorderColor = borde;
            p.FlatAppearance.BorderSize  = 1;
            p.MouseEnter += (s, _) => ((System.Windows.Forms.Button)s!).BackColor =
                System.Drawing.Color.FromArgb(25, borde);
            p.MouseLeave += (s, _) => ((System.Windows.Forms.Button)s!).BackColor = DisGen.CPanel;
            return p;
        }

        private void InitializeComponent()
        {
            lblBienvenido.AutoSize  = true;
            lblBienvenido.Font      = DisGen.Orbitron(10f, System.Drawing.FontStyle.Bold);
            lblBienvenido.ForeColor = DisGen.CCyan;
            lblBienvenido.Location  = new System.Drawing.Point(24, 14);
            lblBienvenido.Text      = "¿Qué desea hacer?";

            // Fila 1: 4 botones (ancho 215 + gap 10 = 225 * 4 = 900, cabe en 958)
            var bRetiro   = CrearOpcion("💰","RETIRO",     "Efectivo",    DisGen.CCyan,    10,  50); bRetiro.Click   += BtnRetiro_Click;
            var bSaldo    = CrearOpcion("📊","SALDO",      "Consulta",    DisGen.COro,    235,  50); bSaldo.Click += BtnSaldo_Click; bSaldo.Tag = "saldo";
            var bDeposito = CrearOpcion("📥","DEPÓSITO",   "Efectivo",    DisGen.CVerde,  460,  50); bDeposito.Click += BtnDeposito_Click;
            var bTransfer = CrearOpcion("↗","TRANSFERIR", "Enviar",      DisGen.CCyan,   685,  50); bTransfer.Click += BtnTransferencia_Click;
            // Fila 2: 3 botones centrados
            var bHistorial= CrearOpcion("🔄","HISTORIAL", "Movimientos", DisGen.COro,     10, 182); bHistorial.Click+= BtnHistorial_Click;
            var bNIP      = CrearOpcion("🔑","CAMBIO NIP","Cambiar",     DisGen.CRojo,   235, 182); bNIP.Click      += BtnCambioNIP_Click;
            var bPago     = CrearOpcion("💳","SERVICIOS", "Pagar",       DisGen.CCyan,   460, 182); bPago.Click     += BtnPagoServicios_Click;

            this.SuspendLayout();
            this.Name = "FrmMenu";
            this.Text = "BankIA — Menú Principal";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblBienvenido, bRetiro, bSaldo, bDeposito, bTransfer, bHistorial, bNIP, bPago });
            this.ResumeLayout(false);
        }
    }
}

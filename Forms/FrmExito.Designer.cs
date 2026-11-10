namespace BankIA.Forms
{
    partial class FrmExito
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Label lblReferencia    = new();
        internal System.Windows.Forms.Label lblOperacion     = new();
        internal System.Windows.Forms.Label lblMonto         = new();
        internal System.Windows.Forms.Label lblSaldoRestante = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ✓ más pequeño y bien separado del texto
            var lblCheck = new System.Windows.Forms.Label
            {
                Text = "✓", Font = new System.Drawing.Font("Segoe UI", 30f),
                ForeColor = DisGen.CVerde, AutoSize = true,
                Location = new System.Drawing.Point(450, 18)
            };
            var lblExito = new System.Windows.Forms.Label
            {
                Text = "OPERACIÓN EXITOSA",
                Font = DisGen.Orbitron(13f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CVerde, AutoSize = true,
                Location = new System.Drawing.Point(260, 85)
            };
            lblReferencia.AutoSize = true;
            lblReferencia.Font = DisGen.Exo2(9f);
            lblReferencia.ForeColor = DisGen.CMuted;
            lblReferencia.Location = new System.Drawing.Point(310, 122);

            // Recibo
            var pRecibo = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(180, 152),
                Size = new System.Drawing.Size(580, 150),
                BackColor = DisGen.CPanel,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            var lblOpL  = new System.Windows.Forms.Label { Text = "Operación:",     AutoSize = true, Font = DisGen.Exo2(9f),  ForeColor = DisGen.CMuted,  Location = new System.Drawing.Point(16, 16) };
            var lblMonL = new System.Windows.Forms.Label { Text = "Monto:",         AutoSize = true, Font = DisGen.Exo2(9f),  ForeColor = DisGen.CMuted,  Location = new System.Drawing.Point(16, 60) };
            var lblSalL = new System.Windows.Forms.Label { Text = "Saldo restante:",AutoSize = true, Font = DisGen.Exo2(9f),  ForeColor = DisGen.CMuted,  Location = new System.Drawing.Point(16, 104) };

            lblOperacion.AutoSize    = true; lblOperacion.Font    = DisGen.Exo2(10f, System.Drawing.FontStyle.Bold); lblOperacion.ForeColor    = DisGen.CBlanco; lblOperacion.Location    = new System.Drawing.Point(200, 16);
            lblMonto.AutoSize        = true; lblMonto.Font        = DisGen.Orbitron(10f); lblMonto.ForeColor        = DisGen.CVerde; lblMonto.Location        = new System.Drawing.Point(200, 56);
            lblSaldoRestante.AutoSize= true; lblSaldoRestante.Font= DisGen.Orbitron(10f); lblSaldoRestante.ForeColor= DisGen.CBlanco; lblSaldoRestante.Location= new System.Drawing.Point(200, 100);

            pRecibo.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblOpL, lblOperacion, lblMonL, lblMonto, lblSalL, lblSaldoRestante });

            // Botones
            var btnOtro = new System.Windows.Forms.Button
            {
                Text = "OTRO TRÁMITE", Location = new System.Drawing.Point(320, 355),
                Size = new System.Drawing.Size(185, 46),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CCyan,
                Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnOtro.FlatAppearance.BorderColor = DisGen.CCyanDim;
            btnOtro.Click += BtnOtroTramite_Click;

            var btnFin = new System.Windows.Forms.Button
            {
                Text = "FINALIZAR", Location = new System.Drawing.Point(524, 355),
                Size = new System.Drawing.Size(140, 46),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CRojo,
                Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnFin.FlatAppearance.BorderColor = DisGen.CRojo;
            btnFin.Click += BtnFinalizar_Click;

            var btnComp = new System.Windows.Forms.Button
            {
                Text = "📄 COMPROBANTE", Location = new System.Drawing.Point(40, 355),
                Size = new System.Drawing.Size(145, 46),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CMuted,
                Font = DisGen.Orbitron(7f), FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnComp.FlatAppearance.BorderColor = DisGen.CBorde;
            btnComp.Click += BtnComprobante_Click;

            var btnCorreo = new System.Windows.Forms.Button
            {
                Text = "✉ CORREO", Location = new System.Drawing.Point(200, 355),
                Size = new System.Drawing.Size(105, 46),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CMuted,
                Font = DisGen.Orbitron(7f), FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Enabled = false
            };
            btnCorreo.FlatAppearance.BorderColor = DisGen.CBorde;

            this.SuspendLayout();
            this.Name = "FrmExito";
            this.Text = "BankIA — Operación Exitosa";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblCheck, lblExito, lblReferencia, pRecibo, btnOtro, btnFin, btnComp, btnCorreo });
            this.ResumeLayout(false);
        }
    }
}

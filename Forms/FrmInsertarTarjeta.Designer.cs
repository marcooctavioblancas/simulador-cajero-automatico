namespace BankIA.Forms
{
    partial class FrmInsertarTarjeta
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.TextBox txtNumeroTarjeta = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label
            {
                Text = "Acerque o inserte su tarjeta", AutoSize = true,
                Font = DisGen.Orbitron(11f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(240, 50)
            };
            var lblNum = new System.Windows.Forms.Label
            {
                Text = "Número de tarjeta (16 dígitos):", AutoSize = true,
                Font = DisGen.Exo2(9f), ForeColor = DisGen.CMuted,
                Location = new System.Drawing.Point(240, 120)
            };
            txtNumeroTarjeta.Location    = new System.Drawing.Point(240, 148);
            txtNumeroTarjeta.Size        = new System.Drawing.Size(380, 34);
            txtNumeroTarjeta.Font        = DisGen.Orbitron(13f);
            txtNumeroTarjeta.MaxLength   = 16;
            txtNumeroTarjeta.BackColor   = DisGen.CSuperficie;
            txtNumeroTarjeta.ForeColor   = DisGen.CBlanco;
            txtNumeroTarjeta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            var btnContinuar = new System.Windows.Forms.Button
            {
                Text = "CONTINUAR →", Location = new System.Drawing.Point(500, 210),
                Size = new System.Drawing.Size(160, 44),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CCyan,
                Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnContinuar.FlatAppearance.BorderColor = DisGen.CCyanDim;
            btnContinuar.Click += BtnContinuar_Click;

            this.SuspendLayout();
            this.Name = "FrmInsertarTarjeta";
            this.Text = "BankIA — Insertar Tarjeta";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblTitulo, lblNum, txtNumeroTarjeta, btnContinuar });
            this.ResumeLayout(false);
        }
    }
}

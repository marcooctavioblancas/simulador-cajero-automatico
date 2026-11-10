namespace BankIA.Forms
{
    partial class FrmDonacion
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button CrearBtnDonacion(string texto, string tag, int x, int y)
        {
            var b = new System.Windows.Forms.Button
            {
                Text = texto, Tag = tag,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(180, 70),
                Font = DisGen.Orbitron(11f, System.Drawing.FontStyle.Bold),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CVerde,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            b.FlatAppearance.BorderColor = DisGen.CVerde;
            b.Click += BtnDonacion_Click;
            return b;
        }

        private void InitializeComponent()
        {
            // Icono de corazón
            var lblIcon = new System.Windows.Forms.Label
            {
                Text = "♥",
                Font = new System.Drawing.Font("Segoe UI", 36f),
                ForeColor = DisGen.CRojo,
                AutoSize = true,
                Location = new System.Drawing.Point(430, 15)
            };

            var lblTitulo = new System.Windows.Forms.Label
            {
                Text = "¿DESEA HACER UNA DONACIÓN?",
                Font = DisGen.Orbitron(13f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CBlanco,
                AutoSize = true,
                Location = new System.Drawing.Point(200, 90)
            };

            var lblOrg = new System.Windows.Forms.Label
            {
                Text = "Cruz Roja Mexicana",
                Font = DisGen.Exo2(12f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CRojo,
                AutoSize = true,
                Location = new System.Drawing.Point(370, 130)
            };

            var lblSub = new System.Windows.Forms.Label
            {
                Text = "Su contribución ayuda a miles de personas cada día.",
                Font = DisGen.Exo2(9f),
                ForeColor = DisGen.CMuted,
                AutoSize = true,
                Location = new System.Drawing.Point(240, 162)
            };

            // Botones de monto
            var b5  = CrearBtnDonacion("$5",  "5",  80,  220);
            var b10 = CrearBtnDonacion("$10", "10", 290, 220);
            var b20 = CrearBtnDonacion("$20", "20", 500, 220);

            // Botón No gracias
            var btnNo = new System.Windows.Forms.Button
            {
                Text = "NO GRACIAS",
                Location = new System.Drawing.Point(710, 220),
                Size = new System.Drawing.Size(180, 70),
                Font = DisGen.Orbitron(9f),
                BackColor = DisGen.CPanel,
                ForeColor = DisGen.CMuted,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnNo.FlatAppearance.BorderColor = DisGen.CBorde;
            btnNo.Click += BtnNoDonacion_Click;

            var lblNota = new System.Windows.Forms.Label
            {
                Text = "La donación se cargará a su cuenta junto con esta operación.",
                Font = DisGen.Exo2(8f),
                ForeColor = DisGen.CMuted,
                AutoSize = true,
                Location = new System.Drawing.Point(180, 320)
            };

            this.SuspendLayout();
            this.Name = "FrmDonacion";
            this.Text = "BankIA — Donación Benéfica";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblIcon, lblTitulo, lblOrg, lblSub,
                b5, b10, b20, btnNo,
                lblNota
            });
            this.ResumeLayout(false);
        }
    }
}

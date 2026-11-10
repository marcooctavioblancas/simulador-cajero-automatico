namespace BankIA.Forms
{
    partial class FrmPagoServicios
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label
            {
                Text      = "Pago de Servicios",
                Font      = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CCyan,
                AutoSize  = true,
                Location  = new System.Drawing.Point(24, 14)
            };
            var lblSub = new System.Windows.Forms.Label
            {
                Text      = "Seleccione el servicio que desea pagar",
                Font      = DisGen.Exo2(9f),
                ForeColor = DisGen.CMuted,
                AutoSize  = true,
                Location  = new System.Drawing.Point(24, 36)
            };

            this.SuspendLayout();
            this.Name = "FrmPagoServicios";
            this.Text = "BankIA — Pago de Servicios";
            this.PanelContenido.Controls.AddRange(
                new System.Windows.Forms.Control[] { lblTitulo, lblSub });
            this.ResumeLayout(false);
        }
    }
}

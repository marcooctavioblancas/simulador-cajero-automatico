namespace BankIA.Forms
{
    partial class FrmPagoServicioDetalle
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Label   lblNombreServicio = new();
        internal System.Windows.Forms.Label   lblInstrucciones  = new();
        internal System.Windows.Forms.Label   lblReferencia     = new();
        internal System.Windows.Forms.TextBox txtReferencia     = new();
        internal System.Windows.Forms.Label   lblMonto          = new();
        internal System.Windows.Forms.TextBox txtMonto          = new();
        internal System.Windows.Forms.Panel   pnlMontos         = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNombreServicio.AutoSize    = true;
            lblNombreServicio.Font        = DisGen.Orbitron(14f, System.Drawing.FontStyle.Bold);
            lblNombreServicio.ForeColor   = DisGen.CCyan;
            lblNombreServicio.Location    = new System.Drawing.Point(24, 14);

            lblInstrucciones.AutoSize     = true;
            lblInstrucciones.Font         = DisGen.Exo2(9f);
            lblInstrucciones.ForeColor    = DisGen.CMuted;
            lblInstrucciones.Location     = new System.Drawing.Point(24, 54);
            lblInstrucciones.MaximumSize  = new System.Drawing.Size(800, 0);

            lblReferencia.AutoSize        = true;
            lblReferencia.Font            = DisGen.Exo2(9f);
            lblReferencia.ForeColor       = DisGen.CMuted;
            lblReferencia.Location        = new System.Drawing.Point(24, 104);

            txtReferencia.Location        = new System.Drawing.Point(24, 126);
            txtReferencia.Size            = new System.Drawing.Size(340, 34);
            txtReferencia.Font            = DisGen.Orbitron(11f);
            txtReferencia.BackColor       = DisGen.CSuperficie;
            txtReferencia.ForeColor       = DisGen.CBlanco;
            txtReferencia.BorderStyle     = System.Windows.Forms.BorderStyle.FixedSingle;

            lblMonto.AutoSize             = true;
            lblMonto.Font                 = DisGen.Exo2(9f);
            lblMonto.ForeColor            = DisGen.CMuted;
            lblMonto.Location             = new System.Drawing.Point(24, 180);
            lblMonto.Text                 = "Monto a pagar:";

            txtMonto.Location             = new System.Drawing.Point(24, 202);
            txtMonto.Size                 = new System.Drawing.Size(220, 34);
            txtMonto.Font                 = DisGen.Orbitron(13f);
            txtMonto.BackColor            = DisGen.CSuperficie;
            txtMonto.ForeColor            = DisGen.COro;
            txtMonto.BorderStyle          = System.Windows.Forms.BorderStyle.FixedSingle;

            pnlMontos.Location            = new System.Drawing.Point(24, 180);
            pnlMontos.Size                = new System.Drawing.Size(860, 80);
            pnlMontos.BackColor           = DisGen.CNavy;

            var btnConf = new System.Windows.Forms.Button
            {
                Text      = "CONFIRMAR",
                Location  = new System.Drawing.Point(600, 375),
                Size      = new System.Drawing.Size(160, 46),
                Font      = DisGen.Orbitron(8f),
                BackColor = DisGen.CPanel,
                ForeColor = DisGen.COro,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            DisGen.EstilarBtnPrimario(btnConf, DisGen.COro);
            btnConf.Click += BtnConfirmar_Click;

            this.SuspendLayout();
            this.Name = "FrmPagoServicioDetalle";
            this.Text = "BankIA — Pago de Servicio";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblNombreServicio, lblInstrucciones, lblReferencia, txtReferencia,
                lblMonto, txtMonto, pnlMontos, btnConf
            });
            this.ResumeLayout(false);
        }
    }
}

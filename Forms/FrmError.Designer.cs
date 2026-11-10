namespace BankIA.Forms
{
    partial class FrmError
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Label  lblMensaje    = new();
        internal System.Windows.Forms.Button btnReintentar = new();
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var lblX     = new System.Windows.Forms.Label { Text = "✕", Font = new System.Drawing.Font("Segoe UI", 42f), ForeColor = DisGen.CRojo, AutoSize = true, Location = new System.Drawing.Point(430, 20) };
            var lblError = new System.Windows.Forms.Label { Text = "OPERACIÓN FALLIDA", Font = DisGen.Orbitron(14f, System.Drawing.FontStyle.Bold), ForeColor = DisGen.CRojo, AutoSize = true, Location = new System.Drawing.Point(285, 100) };
            lblMensaje.AutoSize = true; lblMensaje.Font = DisGen.Exo2(11f); lblMensaje.ForeColor = DisGen.CMuted;
            lblMensaje.Location = new System.Drawing.Point(200, 170); lblMensaje.MaximumSize = new System.Drawing.Size(560, 0);

            var btnCancel = new System.Windows.Forms.Button { Text = "CANCELAR", Location = new System.Drawing.Point(310, 340), Size = new System.Drawing.Size(150, 44), BackColor = DisGen.CPanel, ForeColor = DisGen.CRojo, Font = DisGen.Orbitron(8f), FlatStyle = System.Windows.Forms.FlatStyle.Flat, Cursor = System.Windows.Forms.Cursors.Hand };
            btnCancel.FlatAppearance.BorderColor = DisGen.CRojo; btnCancel.Click += BtnCancelar_Click;

            btnReintentar.Text = "REINTENTAR"; btnReintentar.Location = new System.Drawing.Point(490, 340);
            btnReintentar.Size = new System.Drawing.Size(150, 44); btnReintentar.BackColor = DisGen.CPanel;
            btnReintentar.ForeColor = DisGen.COro; btnReintentar.Font = DisGen.Orbitron(8f);
            btnReintentar.FlatStyle = System.Windows.Forms.FlatStyle.Flat; btnReintentar.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReintentar.FlatAppearance.BorderColor = DisGen.COroDim; btnReintentar.Click += BtnReintentar_Click;

            this.SuspendLayout();
            this.Name = "FrmError"; this.Text = "BankIA — Error";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[] { lblX, lblError, lblMensaje, btnCancel, btnReintentar });
            this.ResumeLayout(false);
        }
    }
}

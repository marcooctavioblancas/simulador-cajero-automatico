namespace BankIA.Forms
{
    partial class FrmNIP
    {
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.Panel pnlDot1 = new();
        internal System.Windows.Forms.Panel pnlDot2 = new();
        internal System.Windows.Forms.Panel pnlDot3 = new();
        internal System.Windows.Forms.Panel pnlDot4 = new();

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button CrearTecla(string texto, string tag, int col, int fila)
        {
            int x = 300 + col * 90, y = 140 + fila * 68;
            var btn = new System.Windows.Forms.Button
            {
                Text = texto, Tag = tag,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(74, 56),
                Font = DisGen.Orbitron(14f, System.Drawing.FontStyle.Bold),
                BackColor = DisGen.CPanel, ForeColor = DisGen.CBlanco,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = DisGen.CBorde;
            return btn;
        }

        private void InitializeComponent()
        {
            var lblTitulo = new System.Windows.Forms.Label
            {
                Text = "Ingrese su NIP", AutoSize = true,
                Font = DisGen.Orbitron(12f, System.Drawing.FontStyle.Bold),
                ForeColor = DisGen.CCyan, Location = new System.Drawing.Point(340, 30)
            };

            // Dots — circulares, pintados en FrmNIP.cs via evento Paint
            int dotY = 85;
            foreach (var (dot, i) in new[] { pnlDot1, pnlDot2, pnlDot3, pnlDot4 }.Select((d,i)=>(d,i)))
            {
                dot.Size        = new System.Drawing.Size(34, 34);
                dot.Location    = new System.Drawing.Point(316 + i * 46, dotY);
                dot.BackColor   = System.Drawing.Color.Transparent;
                dot.BorderStyle = System.Windows.Forms.BorderStyle.None;
                dot.Tag         = false; // false = vacío, true = lleno
            }

            // Teclas numéricas
            var btn1 = CrearTecla("1","1",0,0); btn1.Click += BtnNumero_Click;
            var btn2 = CrearTecla("2","2",1,0); btn2.Click += BtnNumero_Click;
            var btn3 = CrearTecla("3","3",2,0); btn3.Click += BtnNumero_Click;
            var btn4 = CrearTecla("4","4",0,1); btn4.Click += BtnNumero_Click;
            var btn5 = CrearTecla("5","5",1,1); btn5.Click += BtnNumero_Click;
            var btn6 = CrearTecla("6","6",2,1); btn6.Click += BtnNumero_Click;
            var btn7 = CrearTecla("7","7",0,2); btn7.Click += BtnNumero_Click;
            var btn8 = CrearTecla("8","8",1,2); btn8.Click += BtnNumero_Click;
            var btn9 = CrearTecla("9","9",2,2); btn9.Click += BtnNumero_Click;
            var btn0 = CrearTecla("0","0",1,3); btn0.Click += BtnNumero_Click;

            var btnBorrar = CrearTecla("⌫","del",0,3);
            btnBorrar.ForeColor = DisGen.CRojo;
            btnBorrar.FlatAppearance.BorderColor = DisGen.CRojo;
            btnBorrar.Click += BtnBorrar_Click;

            var btnOk = CrearTecla("✓","ok",2,3);
            btnOk.ForeColor = DisGen.CVerde;
            btnOk.FlatAppearance.BorderColor = DisGen.CVerde;
            btnOk.Click += BtnAceptar_Click;

            this.SuspendLayout();
            this.Name = "FrmNIP";
            this.Text = "BankIA — Ingreso de NIP";
            this.PanelContenido.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitulo, pnlDot1, pnlDot2, pnlDot3, pnlDot4,
                btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn0,
                btnBorrar, btnOk
            });
            this.ResumeLayout(false);
        }
    }
}

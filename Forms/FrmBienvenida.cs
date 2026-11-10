using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmBienvenida : DisGen
{
    private readonly System.Windows.Forms.Timer _pulseTimer = new();
    private float _pulsePhase = 0f;

    public FrmBienvenida()
    {
        InitializeComponent();
        MostrarRegresar         = false;
        BtnCerrarSesion.Visible = false;

        if (!DatabaseHelper.TestConnection(out string err))
            MessageBox.Show($"Sin conexión a BD:\n{err}", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Solo navegar si el clic no fue sobre un botón hijo
        this.MouseClick += (_, e) =>
        {
            var ctrl = GetChildAtPoint(e.Location);
            if (ctrl is not Button)
                Navigator.IrA(this, new FrmInsertarTarjeta());
        };
        PanelContenido.MouseClick += (_, e) =>
        {
            var ctrl = PanelContenido.GetChildAtPoint(e.Location);
            if (ctrl is not Button)
                Navigator.IrA(this, new FrmInsertarTarjeta());
        };

        // Centrar contenido al cargar y al cambiar tamaño
        this.Load                      += (_, _) => CentrarYAjustar();
        PanelContenido.SizeChanged     += (_, _) => CentrarYAjustar();

        // Animación de pulso en el texto de insertar tarjeta
        _pulseTimer.Interval = 40; // ~25 fps
        _pulseTimer.Tick    += (_, _) =>
        {
            _pulsePhase += 0.07f;
            if (_pulsePhase > MathF.Tau) _pulsePhase -= MathF.Tau;
            int alpha = (int)(80 + 175 * (0.5f + 0.5f * MathF.Sin(_pulsePhase)));
            _lblInsert.ForeColor = Color.FromArgb(Math.Clamp(alpha, 0, 255), CCyan);
        };
        _pulseTimer.Start();
    }

    private void CentrarYAjustar()
    {
        if (PanelContenido.Width <= 0 || PanelContenido.Height <= 0) return;

        // Ajustar "IA" pegado a "BANK" sin gap (compensar padding interno del Label)
        _lblIA.Location = new Point(_lblBank.Right - 12, _lblBank.Top);

        // Centrar el slogan y textos horizontalmente dentro del panel
        int logoW = _lblIA.Right;
        _lblSlogan.Location = new Point((logoW - _lblSlogan.Width) / 2, _lblSlogan.Top);
        _lblInsert.Location = new Point((logoW - _lblInsert.Width) / 2, _lblInsert.Top);
        _lblFlecha.Location = new Point((logoW - _lblFlecha.Width) / 2, _lblFlecha.Top);

        // Recalcular tamaño del panel — incluir botón SIN TARJETA
        _pnlCentro.Size = new Size(
            Math.Max(logoW, _lblInsert.Right) + 20,
            _lblFlecha.Bottom + 80);

        // Centrar el panel en PanelContenido
        _pnlCentro.Location = new Point(
            (PanelContenido.Width  - _pnlCentro.Width)  / 2,
            (PanelContenido.Height - _pnlCentro.Height) / 2);
    }

    private void FrmBienvenida_Click(object sender, EventArgs e)
        => Navigator.IrA(this, new FrmInsertarTarjeta());

    private void BtnSinTarjeta_Click(object sender, EventArgs e)
        => Navigator.IrA(this, new FrmRetiroSinTarjeta());
}

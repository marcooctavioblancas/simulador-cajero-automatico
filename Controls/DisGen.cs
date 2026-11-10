using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BankIA;

/// <summary>
/// Clase base de todos los formularios de BankIA.
/// Pinta el marco de cajero (borde, esquinas, scanlines, top bar) y
/// expone PanelContenido donde cada formulario agrega sus controles.
/// Siempre corre en pantalla completa y el marco se adapta a cualquier resolución.
/// </summary>
public class DisGen : Form
{
    // ── Paleta de colores ────────────────────────────────────────────────
    public static readonly Color CNegro      = Color.FromArgb(5,   8,  16);
    public static readonly Color CNavy       = Color.FromArgb(8,  15,  30);
    public static readonly Color CSuperficie = Color.FromArgb(13, 26,  46);
    public static readonly Color CPanel      = Color.FromArgb(15, 32,  64);
    public static readonly Color CBorde      = Color.FromArgb(26, 58,  96);
    public static readonly Color CCyan       = Color.FromArgb(0, 212, 255);
    public static readonly Color CCyanDim    = Color.FromArgb(0, 153, 187);
    public static readonly Color COro        = Color.FromArgb(240, 184,  64);
    public static readonly Color COroDim     = Color.FromArgb(201, 144,  32);
    public static readonly Color CBlanco     = Color.FromArgb(232, 244, 255);
    public static readonly Color CMuted      = Color.FromArgb(106, 138, 170);
    public static readonly Color CRojo       = Color.FromArgb(255,  59,  92);
    public static readonly Color CVerde      = Color.FromArgb(0,  232, 122);

    // ── Fuentes privadas ─────────────────────────────────────────────────
    private static PrivateFontCollection? _pfc;
    public  static FontFamily? FOrbitron { get; private set; }
    public  static FontFamily? FExo2     { get; private set; }

    private const int TopBarH  = 52;
    private const int CornerSz = 22;

    // ── Controles compartidos ────────────────────────────────────────────
    protected readonly Panel  PanelContenido  = new();
    private   readonly Label  _lblBanco       = new();
    private   readonly Label  _lblReloj       = new();
    private   readonly Label  _lblBadgeExterno = new(); // badge "• BANCO EXTERNO"
    protected readonly Button BtnRegresar     = new();
    protected readonly Button BtnCerrarSesion = new();
    private   readonly System.Windows.Forms.Timer _timer            = new();
    private   readonly System.Windows.Forms.Timer _animTimer        = new();
    private   readonly System.Windows.Forms.Timer _inactivityTimer  = new();

    // Fase para el glow pulsante (~12 fps)
    private float _glowPhase = 0f;

    // Color de acento externo (púrpura para bancos ajenos)
    public static readonly Color CPurpura = Color.FromArgb(168, 85, 247);

    // ── Color configurable de esquinas ───────────────────────────────────
    private Color _colorEsquinas = CCyan; // CCyan = "usar color de sesión"
    public Color ColorEsquinas
    {
        get => _colorEsquinas;
        set { _colorEsquinas = value; Invalidate(); }
    }

    // Color efectivo: respeta override de formulario (Error=rojo, Éxito=verde),
    // de lo contrario usa el color de sesión (cyan o púrpura si banco externo).
    private Color ColorEfectivo =>
        _colorEsquinas != CCyan
            ? _colorEsquinas
            : Helpers.SessionManager.ColorAcento;

    public bool MostrarRegresar
    {
        get => BtnRegresar.Visible;
        set => BtnRegresar.Visible = value;
    }

    // ── Rectángulo de la pantalla ATM: tamaño fijo máximo, centrado ───────
    // Igual que en el HTML: min(90vw, 960px) × min(76vh, 560px)
    private const int MaxScreenW = 960;
    private const int MaxScreenH = 560;
    private const int NavBarH    = 60;

    private Rectangle ScreenRect
    {
        get
        {
            int w = Math.Min(MaxScreenW, (int)(ClientSize.Width  * 0.92));
            int h = Math.Min(MaxScreenH, (int)((ClientSize.Height - NavBarH - 16) * 0.94));
            int x = (ClientSize.Width  - w) / 2;
            int y = (ClientSize.Height - h - NavBarH) / 2;
            return new Rectangle(x, y, w, h);
        }
    }

    // ── Constructor ──────────────────────────────────────────────────────
    protected DisGen()
    {
        CargarFuentes();
        ConfigForm();
        CrearControles();
        IniciarReloj();
    }

    // ── Configuración base: pantalla completa, sin borde, fondo negro ─────
    private void ConfigForm()
    {
        BackColor        = CNegro;
        ForeColor        = CBlanco;
        FormBorderStyle  = FormBorderStyle.None;
        WindowState      = FormWindowState.Maximized;
        StartPosition    = FormStartPosition.CenterScreen;
        DoubleBuffered   = true;
        KeyPreview       = true;
    }

    // ── Construcción de controles compartidos ────────────────────────────
    private void CrearControles()
    {
        // Badge banco externo (oculto por defecto, se muestra al detectar tarjeta foránea)
        _lblBadgeExterno.AutoSize  = true;
        _lblBadgeExterno.Font      = Exo2(8f, FontStyle.Bold);
        _lblBadgeExterno.ForeColor = CPurpura;
        _lblBadgeExterno.Visible   = false;
        _lblBadgeExterno.Text      = string.Empty;

        // Top bar: nombre banco
        _lblBanco.Text      = "BANKIA · ATM";
        _lblBanco.AutoSize  = true;
        _lblBanco.Font      = Orbitron(9.5f, FontStyle.Bold);
        _lblBanco.ForeColor = CCyan;

        // Top bar: reloj
        _lblReloj.AutoSize  = true;
        _lblReloj.Text      = DateTime.Now.ToString("HH:mm:ss");
        _lblReloj.Font      = Orbitron(8f);
        _lblReloj.ForeColor = CMuted;

        // Panel de contenido
        PanelContenido.BackColor   = CNavy;
        PanelContenido.BorderStyle = BorderStyle.None;
        PanelContenido.Padding     = new Padding(24, 16, 24, 16);

        // Botón ← Regresar
        EstilarNavBtn(BtnRegresar, "← REGRESAR", CCyanDim, CCyan);
        // Botón ✕ Cerrar sesión
        EstilarNavBtn(BtnCerrarSesion, "✕ CERRAR SESIÓN", CRojo, CRojo);

        Controls.AddRange(new Control[]
        {
            _lblBanco, _lblReloj, _lblBadgeExterno,
            PanelContenido,
            BtnRegresar, BtnCerrarSesion
        });
    }

    private void EstilarNavBtn(Button btn, string texto, Color borde, Color fore)
    {
        btn.Text      = texto;
        btn.Size      = new Size(160, 40);
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = CPanel;
        btn.ForeColor = fore;
        btn.Font      = Orbitron(7.5f);
        btn.FlatAppearance.BorderColor = borde;
        btn.FlatAppearance.BorderSize  = 1;
        btn.Cursor    = Cursors.Hand;
    }

    // ── Reposicionado dinámico al cambiar tamaño ─────────────────────────
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        RecalcLayout();
        Invalidate();
    }

    private void RecalcLayout()
    {
        if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
        var sr = ScreenRect;

        PanelContenido.Location = new Point(sr.Left + 1, sr.Top + TopBarH);
        PanelContenido.Size     = new Size(sr.Width - 2, sr.Height - TopBarH - 1);

        _lblBanco.Location       = new Point(sr.Left + 18, sr.Top + 15);
        _lblReloj.Location       = new Point(sr.Right - 110, sr.Top + 16);

        // Badge banco externo: centrado en el top bar
        bool externo = Helpers.SessionManager.EsClienteExterno;
        _lblBadgeExterno.Visible  = externo;
        if (externo)
        {
            _lblBadgeExterno.Text      = $"  ●  {Helpers.SessionManager.NombreBancoExterno}";
            _lblBadgeExterno.ForeColor = CPurpura;
            _lblBadgeExterno.Location  = new Point(
                (sr.Left + sr.Right) / 2 - _lblBadgeExterno.Width / 2,
                sr.Top + 17);
        }

        int navY = sr.Bottom + 10;
        BtnRegresar.Location     = new Point(sr.Left, navY);
        BtnCerrarSesion.Location = new Point(sr.Right - 160, navY);
    }

    private void IniciarReloj()
    {
        _timer.Interval = 1000;
        _timer.Tick    += (_, _) => _lblReloj.Text = DateTime.Now.ToString("HH:mm:ss");
        _timer.Start();

        // Animación del glow: actualiza fase y repinta ~12 fps
        _animTimer.Interval = 80;
        _animTimer.Tick    += (_, _) =>
        {
            _glowPhase += 0.05f;
            if (_glowPhase > MathF.Tau) _glowPhase -= MathF.Tau;
            Invalidate();
        };
        _animTimer.Start();

        // Timeout de inactividad silencioso
        _inactivityTimer.Interval = Config.AppConfig.TimeoutSesionSegundos * 1000;
        _inactivityTimer.Tick    += (_, _) => DispararTimeout();
        _inactivityTimer.Start();
    }

    // Solo el formulario VISIBLE controla el timer — los ocultos lo pausan
    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);
        if (Visible)
            _inactivityTimer.Start();
        else
            _inactivityTimer.Stop();
    }

    private void ReiniciarTimerInactividad()
    {
        if (!Visible) return;
        if (!Helpers.SessionManager.HaySesionActiva) return;
        _inactivityTimer.Stop();
        _inactivityTimer.Start();
    }

    private void DispararTimeout()
    {
        if (!Visible) return;                              // seguridad extra
        if (!Helpers.SessionManager.HaySesionActiva) return;
        _inactivityTimer.Stop();
        int sesID = Helpers.SessionManager.SesionID;
        if (sesID > 0) DAL.SesionDAL.Expirar(sesID);
        Helpers.Navigator.TerminarSesion();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        ReiniciarTimerInactividad();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        ReiniciarTimerInactividad();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        ReiniciarTimerInactividad();
    }

    // ── Dibujo del marco ATM ─────────────────────────────────────────────
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g  = e.Graphics;
        var sr = ScreenRect;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // 1. Gradiente radial de fondo (glow cyan desde abajo del ATM)
        DibujarFondoRadial(g, sr);

        // 2. Fondo de la pantalla ATM
        using (var br = new SolidBrush(CNavy))
            g.FillRectangle(br, sr);

        // 3. Top bar con gradiente propio
        DibujarTopBarFondo(g, sr);

        // 4. Glow pulsante alrededor del borde
        DibujarGlow(g, sr);

        // 5. Borde exterior
        using (var pen = new Pen(Color.FromArgb(90, _colorEsquinas), 2))
            g.DrawRectangle(pen, sr);

        // 6. Línea separadora del top bar
        using (var pen = new Pen(CBorde, 1))
            g.DrawLine(pen, sr.Left, sr.Top + TopBarH, sr.Right, sr.Top + TopBarH);

        // 7. Esquinas decorativas
        DibujarEsquinas(g, sr);

        // 8. Scanlines
        DibujarScanlines(g, sr);
    }

    // Gradiente radial cyan desde la parte inferior del ATM hacia arriba
    private void DibujarFondoRadial(Graphics g, Rectangle sr)
    {
        int rx = sr.Width + 300, ry = sr.Height + 100;
        var ellipseRect = new Rectangle(
            sr.Left + sr.Width / 2 - rx / 2,
            sr.Bottom - ry / 2,
            rx, ry);
        if (ellipseRect.Width <= 0 || ellipseRect.Height <= 0) return;

        try
        {
            using var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(ellipseRect);
            using var pgb = new System.Drawing.Drawing2D.PathGradientBrush(path);
            pgb.CenterPoint    = new PointF(sr.Left + sr.Width / 2f, sr.Bottom - 30f);
            pgb.CenterColor    = Color.FromArgb(28, _colorEsquinas);
            pgb.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
            g.FillRectangle(pgb, ClientRectangle);
        }
        catch { /* PathGradientBrush puede fallar con dimensiones mínimas */ }
    }

    // Gradiente sutil en el top bar para separarlo visualmente
    private void DibujarTopBarFondo(Graphics g, Rectangle sr)
    {
        var r = new Rectangle(sr.Left + 1, sr.Top + 1, sr.Width - 2, TopBarH - 2);
        if (r.Width <= 0 || r.Height <= 0) return;
        using var gb = new System.Drawing.Drawing2D.LinearGradientBrush(r,
            Color.FromArgb(160, 15, 32, 64),
            Color.FromArgb(0, 8, 15, 30),
            System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        g.FillRectangle(gb, r);

        // Acento izquierdo vertical (marca de color)
        using var accentPen = new Pen(_colorEsquinas, 3);
        g.DrawLine(accentPen, sr.Left + 1, sr.Top + 8, sr.Left + 1, sr.Top + TopBarH - 8);
    }

    // Glow animado: múltiples capas con alpha que pulsan en seno
    private void DibujarGlow(Graphics g, Rectangle sr)
    {
        float pulse = 0.10f + 0.05f * (float)Math.Sin(_glowPhase);
        int[]   sizes = { 10, 6, 3 };
        float[] mults = { 0.18f, 0.38f, 0.65f };
        var color = ColorEfectivo;
        for (int i = 0; i < sizes.Length; i++)
        {
            int alpha = (int)(pulse * mults[i] * 255);
            if (alpha <= 0) continue;
            var gr = Rectangle.Inflate(sr, sizes[i], sizes[i]);
            using var pen = new Pen(Color.FromArgb(Math.Min(255, alpha), color), 1f);
            g.DrawRectangle(pen, gr);
        }
    }

    private void DibujarEsquinas(Graphics g, Rectangle sr)
    {
        using var pen = new Pen(ColorEfectivo, 2);
        int sz  = CornerSz;
        int off = 14;
        int l = sr.Left, r = sr.Right;
        int t = sr.Top,  b = sr.Bottom;

        g.DrawLine(pen, l+off, t+off, l+off+sz, t+off);  g.DrawLine(pen, l+off, t+off, l+off, t+off+sz);
        g.DrawLine(pen, r-off, t+off, r-off-sz, t+off);  g.DrawLine(pen, r-off, t+off, r-off, t+off+sz);
        g.DrawLine(pen, l+off, b-off, l+off+sz, b-off);  g.DrawLine(pen, l+off, b-off, l+off, b-off-sz);
        g.DrawLine(pen, r-off, b-off, r-off-sz, b-off);  g.DrawLine(pen, r-off, b-off, r-off, b-off-sz);
    }

    private void DibujarScanlines(Graphics g, Rectangle sr)
    {
        using var pen = new Pen(Color.FromArgb(38, 0, 0, 0), 1);
        for (int y = sr.Top; y < sr.Bottom; y += 4)
            g.DrawLine(pen, sr.Left, y, sr.Right, y);
    }

    // ── Fuentes ───────────────────────────────────────────────────────────
    private static void CargarFuentes()
    {
        if (_pfc is not null) return;
        _pfc = new PrivateFontCollection();

        var assembly = Assembly.GetExecutingAssembly();
        foreach (string res in assembly.GetManifestResourceNames()
            .Where(n => n.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase)))
        {
            using var stream = assembly.GetManifestResourceStream(res);
            if (stream is null) continue;
            byte[] data = new byte[stream.Length];
            _ = stream.Read(data, 0, data.Length);
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try   { _pfc.AddMemoryFont(handle.AddrOfPinnedObject(), data.Length); }
            finally { handle.Free(); }
        }

        FOrbitron = _pfc.Families.FirstOrDefault(f =>
            f.Name.Contains("Orbitron", StringComparison.OrdinalIgnoreCase));
        FExo2 = _pfc.Families.FirstOrDefault(f =>
            f.Name.Contains("Exo", StringComparison.OrdinalIgnoreCase));
    }

    public static Font Orbitron(float size, FontStyle style = FontStyle.Regular)
        => FOrbitron is not null
            ? new Font(FOrbitron, size, style, GraphicsUnit.Point)
            : new Font("Courier New", size, style, GraphicsUnit.Point);

    public static Font Exo2(float size, FontStyle style = FontStyle.Regular)
        => FExo2 is not null
            ? new Font(FExo2, size, style, GraphicsUnit.Point)
            : new Font("Segoe UI", size, style, GraphicsUnit.Point);

    /// <summary>Aplica borde de foco cyan a un TextBox: cyan al entrar, gris al salir.</summary>
    public static void EstilarInput(System.Windows.Forms.TextBox txt)
    {
        txt.Enter += (s, _) =>
        {
            if (s is System.Windows.Forms.TextBox t)
                t.BackColor = Color.FromArgb(18, 36, 64);
        };
        txt.Leave += (s, _) =>
        {
            if (s is System.Windows.Forms.TextBox t)
                t.BackColor = CSuperficie;
        };
    }

    /// <summary>Aplica estilo de botón primario con color de acento fuerte.</summary>
    public static void EstilarBtnPrimario(Button btn, Color color)
    {
        btn.BackColor = Color.FromArgb(40, color);
        btn.ForeColor = color;
        btn.Font      = Orbitron(9f, FontStyle.Bold);
        btn.FlatAppearance.BorderColor = color;
        btn.FlatAppearance.BorderSize  = 2;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) { _timer.Dispose(); _animTimer.Dispose(); _inactivityTimer.Dispose(); }
        base.Dispose(disposing);
    }
}

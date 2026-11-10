using BankIA.Config;
using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmNIP : DisGen
{
    private string _nipIngresado = string.Empty;
    private const int LongitudNIP = 4;

    public FrmNIP()
    {
        InitializeComponent();
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;

        // Hacer los dots circulares con suavizado
        foreach (var dot in new[] { pnlDot1, pnlDot2, pnlDot3, pnlDot4 })
        {
            dot.Paint += PintarDot;
            dot.BackColor = Color.Transparent; // el paint maneja el fondo
        }
    }

    private void PintarDot(object? sender, PaintEventArgs e)
    {
        if (sender is not Panel dot) return;
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        bool lleno = dot.Tag is bool b && b;
        var color  = lleno ? CCyan : CSuperficie;
        var rect   = new Rectangle(2, 2, dot.Width - 5, dot.Height - 5);

        using var br  = new SolidBrush(color);
        g.FillEllipse(br, rect);

        using var pen = new Pen(lleno ? CCyan : CBorde, 2);
        g.DrawEllipse(pen, rect);

        // Glow — usa color de sesión (cyan normal, púrpura si banco externo)
        if (lleno)
        {
            var glowColor = Color.FromArgb(60, SessionManager.ColorAcento);
            using var glowPen = new Pen(glowColor, 4);
            g.DrawEllipse(glowPen, Rectangle.Inflate(rect, 2, 2));
        }
    }

    // Llamado por cada boton numerico (1-9, 0)
    private void BtnNumero_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length >= LongitudNIP) return;
        _nipIngresado += ((Button)sender).Tag?.ToString() ?? string.Empty;
        ActualizarDots();
    }

    private void BtnBorrar_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length > 0)
            _nipIngresado = _nipIngresado[..^1];
        ActualizarDots();
    }

    private void BtnAceptar_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length < LongitudNIP)
        {
            MessageBox.Show("Ingrese los 4 dígitos del NIP.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tarjeta = SessionManager.TarjetaActual;
        if (tarjeta is null) { Navigator.TerminarSesion(); return; }

        if (SessionManager.EsClienteExterno)
        {
            VerificarNIPExterno(tarjeta);
            return;
        }

        // ── BankIA normal ─────────────────────────────────────────────────
        bool ok = TarjetaDAL.VerificarNIP(tarjeta.TarjetaID, _nipIngresado, out string mensaje);
        if (ok)
        {
            var cuenta  = CuentaDAL.ObtenerPorID(tarjeta.CuentaID)!;
            var cliente = CuentaDAL.ObtenerCliente(cuenta.ClienteID)!;
            int sesID   = SesionDAL.Abrir(tarjeta.TarjetaID, SessionManager.CajeroID);
            SessionManager.IniciarSesion(tarjeta, cuenta, cliente, sesID);
            Navigator.IrA(this, new FrmMenu());
        }
        else
        {
            bool bloqueada = mensaje.Contains("bloqueada");
            Navigator.IrA(this, new FrmError(mensaje, bloqueada));
            _nipIngresado = string.Empty;
            ActualizarDots();
        }
    }

    /// <summary>Verificación de NIP para tarjetas de banco externo (intentos en memoria).</summary>
    private void VerificarNIPExterno(Models.Tarjeta tarjeta)
    {
        bool ok = SecurityHelper.VerificarNIP(_nipIngresado, tarjeta.NIPHash);
        if (ok)
        {
            var (cuenta, cliente) = TarjetaDAL.ObtenerCuentaClienteExterno(tarjeta.CuentaID);
            if (cuenta is null || cliente is null)
            {
                Navigator.IrA(this, new FrmError("No se pudo obtener la cuenta del banco externo.", false));
                return;
            }
            // SesionID = 0 para externos: no se registra en BankIA.Sesiones
            // (la FK de Sesiones.TarjetaID solo acepta tarjetas propias)
            SessionManager.IniciarSesion(tarjeta, cuenta, cliente, sesionID: 0);
            Navigator.IrA(this, new FrmMenu());
        }
        else
        {
            SessionManager.IntentosNIPExterno++;
            int restantes = Config.AppConfig.MaxIntentosFallidos - SessionManager.IntentosNIPExterno;

            if (restantes <= 0)
            {
                try { TarjetaDAL.BloquearExterno(tarjeta.TarjetaID); } catch { /* best-effort */ }
                Navigator.IrA(this, new FrmError("Tarjeta bloqueada por exceso de intentos.", true));
            }
            else
            {
                Navigator.IrA(this, new FrmError(
                    $"NIP incorrecto. Le quedan {restantes} intento(s).", false));
                _nipIngresado = string.Empty;
                ActualizarDots();
            }
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();

    // Usa Tag bool para marcar lleno/vacío y fuerza repaint del círculo
    private void ActualizarDots()
    {
        var dots = new[] { pnlDot1, pnlDot2, pnlDot3, pnlDot4 };
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].Tag = i < _nipIngresado.Length;
            dots[i].Invalidate();
        }
    }
}

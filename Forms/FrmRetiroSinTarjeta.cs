using BankIA.Config;
using BankIA.DAL;
using BankIA.Helpers;
using BankIA.Models;

namespace BankIA.Forms;

public partial class FrmRetiroSinTarjeta : DisGen
{
    private string  _nipIngresado   = string.Empty;
    private decimal _montoSel       = 0;
    private Cuenta? _cuenta         = null;
    private Dictionary<int, int> _inventario = new();
    private Dictionary<int, int> _desglose   = new();

    private const int LongitudNIP = 4;

    public FrmRetiroSinTarjeta()
    {
        InitializeComponent();
        BtnRegresar.Click     += (_, _) => Close();
        BtnCerrarSesion.Click += (_, _) => Navigator.TerminarSesion();
        _inventario = BilletesDAL.ObtenerInventario(SessionManager.CajeroID);
        MostrarPaso(1);
    }

    // ── Paso 1: validar cuenta ────────────────────────────────────────────
    private void BtnBuscarCuenta_Click(object sender, EventArgs e)
    {
        string numCuenta = txtCuenta.Text.Trim();
        if (string.IsNullOrEmpty(numCuenta))
        {
            MessageBox.Show("Ingrese el número de cuenta.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _cuenta = CuentaDAL.ObtenerPorNumero(numCuenta);
        if (_cuenta is null)
        {
            MessageBox.Show("Cuenta no encontrada.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_cuenta.Estado != "Activa")
        {
            MessageBox.Show("Cuenta bloqueada o inactiva.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        lblTitularDetectado.Text = CuentaDAL.ObtenerCliente(_cuenta.ClienteID)?.NombreCompleto ?? "Cliente";
        MostrarPaso(2);
    }

    // ── Paso 2: NIP ───────────────────────────────────────────────────────
    private void BtnNumero_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length >= LongitudNIP) return;
        _nipIngresado += ((Button)sender).Tag?.ToString() ?? string.Empty;
        ActualizarDots();
    }

    private void BtnBorrar_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length > 0) _nipIngresado = _nipIngresado[..^1];
        ActualizarDots();
    }

    private void BtnVerificarNIP_Click(object sender, EventArgs e)
    {
        if (_nipIngresado.Length < LongitudNIP)
        {
            MessageBox.Show("Ingrese los 4 dígitos del NIP.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Buscar la tarjeta asociada a la cuenta para verificar NIP
        var tarjeta = ObtenerTarjetaDeCuenta(_cuenta!.CuentaID);
        if (tarjeta is null)
        {
            MessageBox.Show("No se encontró tarjeta asociada a esta cuenta.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!SecurityHelper.VerificarNIP(_nipIngresado, tarjeta.NIPHash))
        {
            MessageBox.Show("NIP incorrecto.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _nipIngresado = string.Empty;
            ActualizarDots();
            return;
        }

        // NIP correcto → mostrar selección de monto
        lblSaldoDisp.Text = $"Saldo disponible: ${_cuenta!.SaldoDisponible:N2}  |  Comisión: ${AppConfig.ComisionSinTarjeta:N2}";
        MostrarPaso(3);
    }

    // ── Paso 3: monto y confirmación ──────────────────────────────────────
    private void BtnMonto_Click(object sender, EventArgs e)
    {
        if (decimal.TryParse(((Button)sender).Tag?.ToString(), out decimal m))
        {
            _montoSel     = m;
            txtMonto.Text = m.ToString("N2");
            ActualizarDesglose(m);
        }
    }

    private void TxtMonto_TextChanged(object sender, EventArgs e)
    {
        if (decimal.TryParse(txtMonto.Text, out decimal m))
        {
            _montoSel = m;
            ActualizarDesglose(m);
        }
    }

    private void ActualizarDesglose(decimal monto)
    {
        if (monto <= 0) { lblDesgloseST.Text = string.Empty; return; }
        _desglose = BilletesDAL.CalcularDesglose(monto, _inventario);
        decimal total = monto + AppConfig.ComisionSinTarjeta;

        if (_desglose.Count == 0)
        {
            lblDesgloseST.Text      = $"⚠ Sin billetes para ${monto:N2}";
            lblDesgloseST.ForeColor = CRojo;
        }
        else
        {
            lblDesgloseST.Text      = $"Billetes: {BilletesDAL.FormatearDesglose(_desglose)}   |   Total a cobrar: ${total:N2}";
            lblDesgloseST.ForeColor = CVerde;
        }
    }

    private void BtnConfirmar_Click(object sender, EventArgs e)
    {
        if (_montoSel <= 0)
        {
            MessageBox.Show("Seleccione o ingrese un monto.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_desglose.Count == 0)
        {
            MessageBox.Show($"El cajero no tiene billetes para ${_montoSel:N2}.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        bool ok = MovimientoDAL.RetiroSinTarjeta(
            _cuenta!.CuentaID, _montoSel,
            out string referencia, out string error);

        if (ok)
        {
            BilletesDAL.DescontarBilletes(SessionManager.CajeroID, _desglose);

            var cuentaActualizada = CuentaDAL.ObtenerPorID(_cuenta.CuentaID);

            SessionManager.UltimaReferencia    = referencia;
            SessionManager.UltimaTipoOperacion = $"Retiro sin tarjeta (+${AppConfig.ComisionSinTarjeta:N0} comisión)";
            SessionManager.UltimoMonto         = _montoSel;
            SessionManager.SaldoTrasOperacion  = cuentaActualizada?.SaldoDisponible ?? 0;
            SessionManager.UltimoMovimiento    = new Movimiento
            {
                TipoMovimiento  = "RetiroSinTarjeta",
                Monto           = _montoSel + AppConfig.ComisionSinTarjeta,
                Referencia      = referencia,
                Descripcion     = $"Retiro sin tarjeta (comisión ${AppConfig.ComisionSinTarjeta:N2})",
                FechaMovimiento = DateTime.Now,
                SaldoPosterior  = SessionManager.SaldoTrasOperacion
            };

            Navigator.IrA(this, new FrmExito());
        }
        else
        {
            Navigator.IrA(this, new FrmError(error, false));
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    private static Tarjeta? ObtenerTarjetaDeCuenta(int cuentaID)
    {
        // Reutiliza TarjetaDAL: busca la primera tarjeta activa de la cuenta
        const string sql = @"
            SELECT TarjetaID, CuentaID, NumeroTarjeta, NIPHash,
                   FechaExpiracion, TipoTarjeta, IntentosFallidos,
                   Bloqueada, FechaBloqueo, Activa
            FROM Tarjetas
            WHERE CuentaID = @CID AND Activa = 1 AND Bloqueada = 0";

        using var conn = DAL.DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new Microsoft.Data.SqlClient.SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CID", cuentaID);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new Tarjeta
        {
            TarjetaID       = r.GetInt32(0),
            CuentaID        = r.GetInt32(1),
            NumeroTarjeta   = r.GetString(2),
            NIPHash         = r.GetString(3),
            FechaExpiracion = r.GetDateTime(4),
            TipoTarjeta     = r.GetString(5),
            IntentosFallidos= r.GetInt32(6),
            Bloqueada       = r.GetBoolean(7),
            Activa          = r.GetBoolean(9)
        };
    }

    private void ActualizarDots()
    {
        var dots = new[] { pnlDotST1, pnlDotST2, pnlDotST3, pnlDotST4 };
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].Tag = i < _nipIngresado.Length;
            dots[i].Invalidate();
        }
    }

    private void PintarDot(object? sender, PaintEventArgs e)
    {
        if (sender is not Panel dot) return;
        var g    = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        bool lleno = dot.Tag is bool b && b;
        var color  = lleno ? CCyan : CSuperficie;
        var rect   = new Rectangle(2, 2, dot.Width - 5, dot.Height - 5);
        using var br  = new SolidBrush(color);
        g.FillEllipse(br, rect);
        using var pen = new Pen(lleno ? CCyan : CBorde, 2);
        g.DrawEllipse(pen, rect);
    }

    private void MostrarPaso(int paso)
    {
        pnlPaso1.Visible = paso == 1;
        pnlPaso2.Visible = paso == 2;
        pnlPaso3.Visible = paso == 3;
    }
}

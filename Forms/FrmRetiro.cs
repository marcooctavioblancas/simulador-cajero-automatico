using BankIA.Config;
using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmRetiro : DisGen
{
    private decimal _montoSeleccionado = 0;
    private Dictionary<int, int> _desgloseActual = new();
    private Dictionary<int, int> _inventario = new();

    public FrmRetiro()
    {
        InitializeComponent();
        DisGen.EstilarInput(txtMonto);
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
        CargarLimite();
        _inventario = BilletesDAL.ObtenerInventario(SessionManager.CajeroID);
    }

    private void CargarLimite()
    {
        var cuenta = SessionManager.CuentaActual;
        if (cuenta is null) return;

        if (SessionManager.EsClienteExterno)
        {
            lblLimite.Text      = $"Comisión interbancaria: ${AppConfig.ComisionInterbancaria:N2} MXN por retiro";
            lblLimite.ForeColor = CPurpura;
            return;
        }

        decimal retirosHoy = MovimientoDAL.RetirosDiarios(cuenta.CuentaID);
        decimal disponible = cuenta.LimiteRetiroDiario - retirosHoy;
        lblLimite.Text      = $"Límite diario restante: ${disponible:N2} MXN";
        lblLimite.ForeColor = disponible < 1000 ? CRojo : CMuted;
    }

    // Botones de monto rapido (Tag = "200", "500", etc.)
    private void BtnMonto_Click(object sender, EventArgs e)
    {
        if (decimal.TryParse(((Button)sender).Tag?.ToString(), out decimal m))
        {
            _montoSeleccionado = m;
            txtMonto.Text      = m.ToString("N2");
            ActualizarDesglose(m);
        }
    }

    private void TxtMonto_TextChanged(object sender, EventArgs e)
    {
        if (decimal.TryParse(txtMonto.Text, out decimal m))
        {
            _montoSeleccionado = m;
            ActualizarDesglose(m);
        }
    }

    private void ActualizarDesglose(decimal monto)
    {
        if (monto <= 0) { lblDesglose.Text = string.Empty; return; }

        _desgloseActual = BilletesDAL.CalcularDesglose(monto, _inventario);
        if (_desgloseActual.Count == 0)
        {
            lblDesglose.Text      = $"⚠ No hay billetes disponibles para ${monto:N2}";
            lblDesglose.ForeColor = CRojo;
        }
        else
        {
            lblDesglose.Text      = $"Billetes: {BilletesDAL.FormatearDesglose(_desgloseActual)}";
            lblDesglose.ForeColor = CVerde;
        }
    }

    private void BtnConfirmar_Click(object sender, EventArgs e)
    {
        if (_montoSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione o ingrese un monto.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_desgloseActual.Count == 0)
        {
            MessageBox.Show($"El cajero no tiene billetes para dispensar ${_montoSeleccionado:N2}.\nPruebe con otro monto.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cuenta  = SessionManager.CuentaActual!;
        var tarjeta = SessionManager.TarjetaActual!;

        bool ok;
        string referencia, error;

        if (SessionManager.EsClienteExterno)
        {
            // Retiro interbancario — debita de Banco2 con comisión incluida
            ok = MovimientoDAL.RetiroExterno(
                cuenta.CuentaID, _montoSeleccionado,
                out referencia, out error);
        }
        else
        {
            ok = MovimientoDAL.Retiro(
                cuenta.CuentaID, tarjeta.TarjetaID, SessionManager.CajeroID,
                _montoSeleccionado, out referencia, out error);
        }

        if (ok)
        {
            // Solo refrescar desde BankIA si el cliente es local
            if (!SessionManager.EsClienteExterno)
            {
                var cuentaActualizada = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
                if (cuentaActualizada is not null) SessionManager.RefrescarCuenta(cuentaActualizada);
            }
            // Para externos, RetiroExterno ya actualizó SessionManager.CuentaActual

            string tipoOp = SessionManager.EsClienteExterno
                ? $"Retiro efectivo (+${Config.AppConfig.ComisionInterbancaria:N0} comisión)"
                : "Retiro efectivo";

            SessionManager.UltimaReferencia    = referencia;
            SessionManager.UltimaTipoOperacion = tipoOp;
            SessionManager.UltimoMonto         = _montoSeleccionado;
            SessionManager.SaldoTrasOperacion  = SessionManager.CuentaActual!.SaldoDisponible;
            SessionManager.UltimoMovimiento    = new Models.Movimiento
            {
                TipoMovimiento  = "Retiro",
                Monto           = _montoSeleccionado,
                Referencia      = referencia,
                Descripcion     = "Retiro de efectivo",
                FechaMovimiento = DateTime.Now,
                SaldoPosterior  = SessionManager.SaldoTrasOperacion
            };

            // Descontar billetes del inventario del cajero
            BilletesDAL.DescontarBilletes(SessionManager.CajeroID, _desgloseActual);

            Navigator.IrA(this, new FrmDonacion());
        }
        else
        {
            Navigator.IrA(this, new FrmError(error, false));
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

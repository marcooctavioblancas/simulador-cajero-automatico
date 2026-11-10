using BankIA.DAL;
using BankIA.Helpers;
using BankIA.Models;

namespace BankIA.Forms;

public partial class FrmPagoServicioDetalle : DisGen
{
    private readonly Servicio _servicio;
    private decimal _montoSeleccionado = 0;

    public FrmPagoServicioDetalle(Servicio servicio)
    {
        _servicio = servicio;
        InitializeComponent();
        BtnRegresar.Click     += (_, _) => Close();
        BtnCerrarSesion.Click += (_, _) => Navigator.TerminarSesion();
        ConfigurarUI();
    }

    private void ConfigurarUI()
    {
        lblNombreServicio.Text = $"{_servicio.Icono}  {_servicio.Nombre}";
        lblInstrucciones.Text  = _servicio.Instrucciones ?? string.Empty;

        if (_servicio.EsRecarga)
        {
            lblReferencia.Text = "Número de teléfono (10 dígitos):";
            lblMonto.Visible   = false;
            txtMonto.Visible   = false;
            CargarBotonesMontos();
        }
        else
        {
            pnlMontos.Visible = false;
        }
    }

    private void CargarBotonesMontos()
    {
        var montos = ServicioDAL.ObtenerMontos(_servicio.ServicioID);
        int x = 0;
        foreach (var m in montos)
        {
            var btn = new Button
            {
                Text      = $"${m.Monto:N0}\n{m.Descripcion}",
                Tag       = m.Monto,
                Location  = new Point(x, 0),
                Size      = new Size(150, 70),
                Font      = Orbitron(9f),
                BackColor = CPanel,
                ForeColor = COro,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = COroDim;
            btn.Click += (sender, _) =>
            {
                _montoSeleccionado = (decimal)((Button)sender!).Tag!;
                foreach (Control c in pnlMontos.Controls)
                    if (c is Button b) b.BackColor = CPanel;
                ((Button)sender).BackColor = Color.FromArgb(50, 240, 184, 64);
            };
            pnlMontos.Controls.Add(btn);
            x += 160;
        }
    }

    private void BtnConfirmar_Click(object sender, EventArgs e)
    {
        string referencia = txtReferencia.Text.Trim();
        if (string.IsNullOrEmpty(referencia))
        {
            MessageBox.Show("Ingrese la referencia.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        decimal monto;
        if (_servicio.EsRecarga)
        {
            monto = _montoSeleccionado;
            if (monto <= 0)
            {
                MessageBox.Show("Seleccione un monto de recarga.", "BankIA",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        else
        {
            if (!decimal.TryParse(txtMonto.Text.Replace("$", "").Trim(), out monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto válido.", "BankIA",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        var cuenta  = SessionManager.CuentaActual!;
        var tarjeta = SessionManager.TarjetaActual!;

        bool ok = MovimientoDAL.PagoServicio(
            cuenta.CuentaID, tarjeta.TarjetaID, SessionManager.CajeroID,
            monto, _servicio.Nombre, referencia,
            out string refMov, out string error);

        if (ok)
        {
            var fresca = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
            if (fresca is not null) SessionManager.RefrescarCuenta(fresca);

            SessionManager.UltimaTipoOperacion = $"Pago {_servicio.Nombre}";
            SessionManager.UltimoMonto         = monto;
            SessionManager.UltimaReferencia    = refMov;
            SessionManager.SaldoTrasOperacion  = SessionManager.CuentaActual!.SaldoDisponible;
            SessionManager.UltimoMovimiento    = new Movimiento
            {
                TipoMovimiento  = "PagoServicio",
                Monto           = monto,
                Referencia      = refMov,
                Descripcion     = $"Pago {_servicio.Nombre} · Ref {referencia}",
                FechaMovimiento = DateTime.Now,
                SaldoPosterior  = SessionManager.SaldoTrasOperacion
            };

            Navigator.IrA(this, new FrmDonacion());
        }
        else
        {
            Navigator.IrA(this, new FrmError(error, false));
        }
    }
}

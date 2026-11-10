using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmDeposito : DisGen
{
    public FrmDeposito()
    {
        InitializeComponent();
        DisGen.EstilarInput(txtMonto);
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void BtnDepositar_Click(object sender, EventArgs e)
    {
        if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
        {
            MessageBox.Show("Ingrese un monto válido.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cuenta  = SessionManager.CuentaActual!;
        var tarjeta = SessionManager.TarjetaActual!;

        bool ok = MovimientoDAL.Deposito(
            cuenta.CuentaID, tarjeta.TarjetaID, SessionManager.CajeroID,
            monto, out string referencia, out string error);

        if (ok)
        {
            var cuentaActualizada = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
            if (cuentaActualizada is not null) SessionManager.RefrescarCuenta(cuentaActualizada);

            SessionManager.UltimaReferencia    = referencia;
            SessionManager.UltimaTipoOperacion = "Depósito efectivo";
            SessionManager.UltimoMonto         = monto;
            SessionManager.SaldoTrasOperacion  = SessionManager.CuentaActual!.SaldoDisponible;
            SessionManager.UltimoMovimiento    = new Models.Movimiento
            {
                TipoMovimiento  = "Deposito",
                Monto           = monto,
                Referencia      = referencia,
                Descripcion     = "Depósito de efectivo",
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

    private void BtnCancelar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

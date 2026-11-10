using BankIA.Helpers;
using BankIA.DAL;

namespace BankIA.Forms;

public partial class FrmSaldo : DisGen
{
    public FrmSaldo()
    {
        InitializeComponent();
        BtnRegresar.Click     += BtnSalir_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
        CargarSaldo();
    }

    private void CargarSaldo()
    {
        var cuenta = SessionManager.CuentaActual;
        if (cuenta is null) { Close(); return; }

        // Refrescar desde BD (el saldo puede haber cambiado)
        var fresca = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
        if (fresca is not null) SessionManager.RefrescarCuenta(fresca);

        lblSaldoDisponible.Text = $"${SessionManager.CuentaActual!.SaldoDisponible:N2}";
        lblSaldoContable.Text   = $"${SessionManager.CuentaActual!.SaldoContable:N2}";
        lblNumeroCuenta.Text    = $"Cuenta {SessionManager.CuentaActual!.NumeroCuentaMascarado} · {SessionManager.CuentaActual!.TipoCuenta}";
    }

    private void BtnMovimientos_Click(object sender, EventArgs e)
        => Navigator.IrA(this, new FrmHistorial());

    private void BtnSalir_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

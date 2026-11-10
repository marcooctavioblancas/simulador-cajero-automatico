using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmDonacion : DisGen
{
    public FrmDonacion()
    {
        InitializeComponent();
        MostrarRegresar = false;
        BtnCerrarSesion.Click += (_, _) => Navigator.TerminarSesion();
    }

    private void DonarYContinuar(decimal monto)
    {
        var cuenta  = SessionManager.CuentaActual!;
        var tarjeta = SessionManager.TarjetaActual!;

        bool ok = MovimientoDAL.Donacion(
            cuenta.CuentaID, tarjeta.TarjetaID, SessionManager.CajeroID,
            monto, out string referencia, out string error);

        if (ok)
        {
            var fresca = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
            if (fresca is not null) SessionManager.RefrescarCuenta(fresca);
        }
        // Independientemente de si la donación fue exitosa, avanzar al éxito de la op original
        Navigator.IrA(this, new FrmExito());
    }

    private void BtnDonacion_Click(object sender, EventArgs e)
    {
        if (decimal.TryParse(((Button)sender).Tag?.ToString(), out decimal monto))
            DonarYContinuar(monto);
    }

    private void BtnNoDonacion_Click(object sender, EventArgs e)
        => Navigator.IrA(this, new FrmExito());
}

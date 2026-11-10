using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmCambioNIP : DisGen
{
    public FrmCambioNIP()
    {
        InitializeComponent();
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void BtnContinuar_Click(object sender, EventArgs e)
    {
        string nipActual    = txtNIPActual.Text.Trim();
        string nipNuevo     = txtNIPNuevo.Text.Trim();
        string nipConfirmar = txtNIPConfirmar.Text.Trim();

        if (nipActual.Length != 4 || nipNuevo.Length != 4 || nipConfirmar.Length != 4)
        {
            MessageBox.Show("Todos los NIPs deben tener 4 dígitos.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (nipNuevo != nipConfirmar)
        {
            MessageBox.Show("El nuevo NIP y la confirmación no coinciden.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tarjeta = SessionManager.TarjetaActual!;
        bool nipActualOk = TarjetaDAL.VerificarNIP(tarjeta.TarjetaID, nipActual, out string msg);
        if (!nipActualOk)
        {
            MessageBox.Show(msg, "BankIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        bool ok = TarjetaDAL.CambiarNIP(tarjeta.TarjetaID, nipNuevo, SessionManager.CajeroID);
        if (ok)
        {
            SessionManager.UltimaReferencia    = DAL.DatabaseHelper.GenerarReferencia();
            SessionManager.UltimaTipoOperacion = "Cambio de NIP";
            SessionManager.UltimoMonto         = 0;
            SessionManager.SaldoTrasOperacion  = SessionManager.CuentaActual!.SaldoDisponible;
            Navigator.IrA(this, new FrmExito());
        }
        else
        {
            Navigator.IrA(this, new FrmError("No se pudo cambiar el NIP.", false));
        }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

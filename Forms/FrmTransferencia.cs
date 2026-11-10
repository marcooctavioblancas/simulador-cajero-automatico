using BankIA.DAL;
using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmTransferencia : DisGen
{
    public FrmTransferencia()
    {
        InitializeComponent();
        DisGen.EstilarInput(txtCuentaDestino);
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void BtnConfirmar_Click(object sender, EventArgs e)
    {
        string numeroCuentaDestino = txtCuentaDestino.Text.Trim();
        if (string.IsNullOrEmpty(numeroCuentaDestino))
        {
            MessageBox.Show("Ingrese la cuenta destino.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
        {
            MessageBox.Show("Ingrese un monto válido.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cuentaDestino = CuentaDAL.ObtenerPorNumero(numeroCuentaDestino);
        if (cuentaDestino is null)
        {
            MessageBox.Show("Cuenta destino no encontrada.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Mostrar titular para confirmacion
        var titular = CuentaDAL.ObtenerCliente(cuentaDestino.ClienteID);
        lblTitular.Text = titular?.NombreCompleto ?? "Desconocido";

        var cuenta  = SessionManager.CuentaActual!;
        var tarjeta = SessionManager.TarjetaActual!;

        bool ok = MovimientoDAL.Transferencia(
            cuenta.CuentaID, cuentaDestino.CuentaID, tarjeta.TarjetaID,
            SessionManager.CajeroID, monto, out string referencia, out string error);

        if (ok)
        {
            var cuentaActualizada = CuentaDAL.ObtenerPorID(cuenta.CuentaID);
            if (cuentaActualizada is not null) SessionManager.RefrescarCuenta(cuentaActualizada);

            SessionManager.UltimaReferencia    = referencia;
            SessionManager.UltimaTipoOperacion = "Transferencia";
            SessionManager.UltimoMonto         = monto;
            SessionManager.SaldoTrasOperacion  = SessionManager.CuentaActual!.SaldoDisponible;
            SessionManager.UltimoMovimiento    = new Models.Movimiento
            {
                TipoMovimiento  = "Transferencia",
                Monto           = monto,
                Referencia      = referencia,
                Descripcion     = $"Transferencia a {cuentaDestino.NumeroCuentaMascarado}",
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

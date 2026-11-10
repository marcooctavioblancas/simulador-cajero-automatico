using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmExito : DisGen
{
    public FrmExito()
    {
        InitializeComponent();
        ColorEsquinas    = CVerde;
        MostrarRegresar  = false;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;

        lblReferencia.Text    = $"Referencia: #{SessionManager.UltimaReferencia}";
        lblOperacion.Text     = SessionManager.UltimaTipoOperacion;
        lblMonto.Text         = $"${SessionManager.UltimoMonto:N2} MXN";
        lblSaldoRestante.Text = $"${SessionManager.SaldoTrasOperacion:N2} MXN";
    }

    private void BtnOtroTramite_Click(object sender, EventArgs e)
        => Navigator.VolverAlMenu();

    private void BtnComprobante_Click(object sender, EventArgs e)
    {
        var m      = SessionManager.UltimoMovimiento;
        var cuenta = SessionManager.CuentaActual;
        if (m is null || cuenta is null) return;

        string? path = ComprobanteHelper.Generar(m, cuenta);
        if (path is not null) ComprobanteHelper.AbrirPDF(path);
    }

    private void BtnFinalizar_Click(object sender, EventArgs e)
    {
        DAL.SesionDAL.Cerrar(SessionManager.SesionID);
        Navigator.TerminarSesion();
    }

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
    {
        DAL.SesionDAL.Cerrar(SessionManager.SesionID);
        Navigator.TerminarSesion();
    }
}

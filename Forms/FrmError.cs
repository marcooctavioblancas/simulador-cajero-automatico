using BankIA.Helpers;

namespace BankIA.Forms;

public partial class FrmError : DisGen
{
    private readonly bool _bloqueada;

    public FrmError(string mensaje, bool tarjetaBloqueada)
    {
        InitializeComponent();
        ColorEsquinas    = CRojo;
        MostrarRegresar  = false;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;

        _bloqueada            = tarjetaBloqueada;
        lblMensaje.Text       = mensaje;
        btnReintentar.Visible = !tarjetaBloqueada;
    }

    private void BtnReintentar_Click(object sender, EventArgs e) => Close();

    private void BtnCancelar_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

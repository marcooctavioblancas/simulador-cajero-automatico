using BankIA.DAL;
using BankIA.Helpers;
using BankIA.Config;
using Microsoft.Data.SqlClient;

namespace BankIA.Forms;

public partial class FrmInsertarTarjeta : DisGen
{
    public FrmInsertarTarjeta()
    {
        InitializeComponent();
        DisGen.EstilarInput(txtNumeroTarjeta);
        BtnRegresar.Click     += BtnCancelar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
    }

    private void BtnContinuar_Click(object sender, EventArgs e)
    {
        string numero = txtNumeroTarjeta.Text.Trim().Replace(" ", "");
        if (numero.Length < 12)
        {
            MessageBox.Show("Ingrese el número de tarjeta.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Busca en BankIA primero, luego en bancos externos
        var (tarjeta, esExterno) = TarjetaDAL.ObtenerGlobal(numero);

        if (tarjeta is null)
        {
            MessageBox.Show("Tarjeta no reconocida.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (tarjeta.Bloqueada)
        {
            MessageBox.Show("Tarjeta bloqueada. Acuda a su banco.", "BankIA",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Siempre resetear el estado externo — evita que quede true de sesión anterior
        SessionManager.EsClienteExterno   = esExterno;
        SessionManager.ColorAcento        = esExterno ? CPurpura : CCyan;

        if (esExterno)
        {
            SessionManager.NombreBancoExterno     = AppConfig.NombreBDExterna;
            SessionManager.IntentosNIPExterno     = 0;
            SessionManager.BancoExternoDisponible = VerificarDisponibilidadBancoExterno();
            Invalidate();
        }
        else
        {
            SessionManager.NombreBancoExterno     = string.Empty;
            SessionManager.BancoExternoDisponible = true;
        }

        SessionManager.IniciarSesion(tarjeta, null!, null!, 0);
        Navigator.IrA(this, new FrmNIP());
    }

    /// <summary>Verifica si la BD externa responde en el timeout configurado.</summary>
    private static bool VerificarDisponibilidadBancoExterno()
    {
        try
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                $"SELECT TOP 1 1 FROM [{AppConfig.NombreBDExterna}].dbo.Tarjetas", conn);
            cmd.CommandTimeout = AppConfig.TimeoutBancoExterno;
            cmd.ExecuteScalar();
            return true;
        }
        catch { return false; }
    }

    private void BtnCancelar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

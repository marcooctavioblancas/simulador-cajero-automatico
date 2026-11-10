using BankIA.Helpers;
using BankIA.Config;

namespace BankIA.Forms;

public partial class FrmMenu : DisGen
{
    public FrmMenu()
    {
        InitializeComponent();
        MostrarRegresar       = false;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;

        var cliente = SessionManager.ClienteActual;
        if (cliente is null) return;

        string nombre = cliente.NombreCompleto.Split(' ')[0];

        if (SessionManager.EsClienteExterno)
        {
            bool online = SessionManager.BancoExternoDisponible;
            string modo = online
                ? $"+${AppConfig.ComisionInterbancaria:N0} comisión/op"
                : "MODO CONSULTA — sin conexión";

            lblBienvenido.Text      = $"Bienvenido, {nombre}  ·  {SessionManager.NombreBancoExterno}  ({modo})";
            lblBienvenido.ForeColor = CPurpura;
        }
        else
        {
            lblBienvenido.Text = $"Bienvenido, {nombre}";
        }
    }

    // ── Helpers de bloqueo ────────────────────────────────────────────────

    private static bool PermiteOperacion(bool requiereConexion = true)
    {
        if (!SessionManager.EsClienteExterno) return true;

        if (!SessionManager.BancoExternoDisponible)
        {
            MessageBox.Show(
                "Servicio interbancario no disponible.\nSolo puede consultar su saldo.",
                "BankIA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        if (!requiereConexion) return true;

        // Para banco externo con conexión solo se permite retiro
        return true;
    }

    private static bool EsOperacionBloqueadaParaExterno(string operacion)
    {
        if (!SessionManager.EsClienteExterno) return false;

        MessageBox.Show(
            $"{operacion} no está disponible para tarjetas de otros bancos.",
            "BankIA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return true;
    }

    // ── Handlers de botones ───────────────────────────────────────────────

    private void BtnRetiro_Click(object sender, EventArgs e)
    {
        if (!PermiteOperacion()) return;
        Navigator.IrA(this, new FrmRetiro());
    }

    private void BtnSaldo_Click(object sender, EventArgs e)
        => Navigator.IrA(this, new FrmSaldo());

    private void BtnDeposito_Click(object sender, EventArgs e)
    {
        if (EsOperacionBloqueadaParaExterno("Depósito")) return;
        Navigator.IrA(this, new FrmDeposito());
    }

    private void BtnTransferencia_Click(object sender, EventArgs e)
    {
        if (EsOperacionBloqueadaParaExterno("Transferencia")) return;
        Navigator.IrA(this, new FrmTransferencia());
    }

    private void BtnHistorial_Click(object sender, EventArgs e)
    {
        if (!PermiteOperacion(requiereConexion: false)) return;
        Navigator.IrA(this, new FrmHistorial());
    }

    private void BtnCambioNIP_Click(object sender, EventArgs e)
    {
        if (EsOperacionBloqueadaParaExterno("Cambio de NIP")) return;
        Navigator.IrA(this, new FrmCambioNIP());
    }

    private void BtnPagoServicios_Click(object sender, EventArgs e)
    {
        if (EsOperacionBloqueadaParaExterno("Pago de servicios")) return;
        Navigator.IrA(this, new FrmPagoServicios());
    }

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
    {
        var sesID = SessionManager.SesionID;
        if (sesID > 0) DAL.SesionDAL.Cerrar(sesID);
        Navigator.TerminarSesion();
    }
}

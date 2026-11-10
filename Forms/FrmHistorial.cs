using BankIA.DAL;
using BankIA.Helpers;
using BankIA.Models;

namespace BankIA.Forms;

public partial class FrmHistorial : DisGen
{
    private List<Movimiento> _movimientos = new();

    public FrmHistorial()
    {
        InitializeComponent();
        BtnRegresar.Click     += BtnRegresar_Click;
        BtnCerrarSesion.Click += BtnCerrarSesion_Click;
        CargarHistorial();

        lstMovimientos.SelectedIndexChanged += (_, _) =>
            btnComprobante.Enabled = lstMovimientos.SelectedIndex >= 0;
    }

    private void CargarHistorial()
    {
        var cuenta = SessionManager.CuentaActual;
        if (cuenta is null) { Close(); return; }

        _movimientos = MovimientoDAL.ObtenerUltimos(cuenta.CuentaID, 10);
        lstMovimientos.Items.Clear();

        foreach (var m in _movimientos)
        {
            string linea = $"{m.FechaMovimiento:dd/MM/yyyy HH:mm}  {m.TipoMovimiento,-16}  {m.MontoFormateado,12}";
            lstMovimientos.Items.Add(linea);
        }
    }

    private void BtnComprobante_Click(object sender, EventArgs e)
    {
        int idx = lstMovimientos.SelectedIndex;
        if (idx < 0 || idx >= _movimientos.Count) return;

        var m      = _movimientos[idx];
        var cuenta = SessionManager.CuentaActual;
        if (cuenta is null) return;

        string? path = ComprobanteHelper.Generar(m, cuenta);
        if (path is not null) ComprobanteHelper.AbrirPDF(path);
    }

    private void BtnRegresar_Click(object sender, EventArgs e) => Close();

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
        => Navigator.TerminarSesion();
}

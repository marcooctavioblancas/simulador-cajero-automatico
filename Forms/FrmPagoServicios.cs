using BankIA.DAL;
using BankIA.Helpers;
using BankIA.Models;

namespace BankIA.Forms;

public partial class FrmPagoServicios : DisGen
{
    public FrmPagoServicios()
    {
        InitializeComponent();
        BtnRegresar.Click     += (_, _) => Close();
        BtnCerrarSesion.Click += (_, _) => Navigator.TerminarSesion();
        CargarServicios();
    }

    private void CargarServicios()
    {
        var servicios = ServicioDAL.ObtenerActivos();
        const int btnW = 195, btnH = 95, padX = 24, padY = 55, gapX = 10, gapY = 10;
        int col = 0, row = 0;

        foreach (var s in servicios)
        {
            var btn = new Button
            {
                Text      = $"{s.Icono}\n{s.Nombre}",
                Tag       = s,
                Location  = new Point(padX + col * (btnW + gapX), padY + row * (btnH + gapY)),
                Size      = new Size(btnW, btnH),
                Font      = Orbitron(9f),
                BackColor = CPanel,
                ForeColor = s.EsRecarga ? COro : CCyan,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = s.EsRecarga ? COroDim : CCyanDim;
            btn.Click += (sender, _) =>
            {
                var servicio = (Servicio)((Button)sender!).Tag!;
                Navigator.IrA(this, new FrmPagoServicioDetalle(servicio));
            };
            PanelContenido.Controls.Add(btn);
            col++;
            if (col >= 4) { col = 0; row++; }
        }
    }
}

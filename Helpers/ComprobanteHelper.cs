using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BankIA.Models;

namespace BankIA.Helpers;

internal static class ComprobanteHelper
{
    static ComprobanteHelper()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Abre SaveFileDialog y genera el PDF del movimiento.
    /// Retorna la ruta guardada, o null si el usuario canceló.
    /// </summary>
    public static string? Generar(Movimiento m, Cuenta cuenta)
    {
        using var dialog = new SaveFileDialog
        {
            Title      = "Guardar comprobante",
            FileName   = $"Comprobante_{m.Referencia}.pdf",
            Filter     = "Archivo PDF|*.pdf",
            DefaultExt = "pdf"
        };

        if (dialog.ShowDialog() != DialogResult.OK) return null;

        string path = dialog.FileName;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Segoe UI"));

                // ── Encabezado ───────────────────────────────────────
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem()
                            .Text("BANKIA")
                            .FontSize(30).Bold()
                            .FontColor(Colors.Cyan.Medium);
                        row.ConstantItem(200).AlignRight()
                            .Text("Comprobante de Operación")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);
                    });
                    col.Item().PaddingTop(4)
                        .LineHorizontal(1).LineColor(Colors.Cyan.Medium);
                });

                // ── Contenido ─────────────────────────────────────────
                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Item()
                        .Background(Colors.Grey.Lighten4)
                        .Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                        .Padding(14)
                        .Column(inner =>
                        {
                            Fila(inner, "Tipo de operación", m.TipoMovimiento);
                            Fila(inner, "Monto",
                                $"${m.Monto:N2} MXN", negrita: true);
                            Fila(inner, "Referencia", m.Referencia);
                            Fila(inner, "Fecha y hora",
                                m.FechaMovimiento.ToString("dd/MM/yyyy HH:mm:ss"));
                            Fila(inner, "Cuenta", cuenta.NumeroCuentaMascarado);
                            Fila(inner, "Tipo de cuenta", cuenta.TipoCuenta);
                            Fila(inner, "Saldo resultante",
                                $"${m.SaldoPosterior:N2} MXN");
                            if (!string.IsNullOrEmpty(m.Descripcion))
                                Fila(inner, "Descripción", m.Descripcion);
                        });
                });

                // ── Pie ───────────────────────────────────────────────
                page.Footer().AlignCenter()
                    .Text("Este documento es un comprobante informativo. BankIA — Inteligencia Financiera.")
                    .FontSize(8).FontColor(Colors.Grey.Medium);
            });
        })
        .GeneratePdf(path);

        return path;
    }

    public static void AbrirPDF(string path)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName        = path,
            UseShellExecute = true
        });
    }

    private static void Fila(ColumnDescriptor col, string clave, string valor, bool negrita = false)
    {
        col.Item().Row(row =>
        {
            row.ConstantItem(185)
                .Text(clave)
                .FontColor(Colors.Grey.Darken1);
            var txt = row.RelativeItem().Text(valor);
            if (negrita) txt.Bold();
        });
        col.Item().PaddingBottom(5)
            .LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
    }
}

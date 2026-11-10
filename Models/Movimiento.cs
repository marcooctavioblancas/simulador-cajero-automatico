namespace BankIA.Models;

internal class Movimiento
{
    public int      MovimientoID    { get; set; }
    public int      CuentaID        { get; set; }
    public int?     TarjetaID       { get; set; }
    public int?     CajeroID        { get; set; }
    public string   TipoMovimiento  { get; set; } = string.Empty;
    public decimal? Monto           { get; set; }
    public decimal  SaldoAnterior   { get; set; }
    public decimal  SaldoPosterior  { get; set; }
    public int?     CuentaDestinoID { get; set; }
    public string   Referencia      { get; set; } = string.Empty;
    public string?  Descripcion     { get; set; }
    public string   Estado          { get; set; } = "Exitoso";
    public bool     EsInterbancario { get; set; }
    public DateTime FechaMovimiento { get; set; }

    // Para mostrar en historial: +/- segun tipo
    public string MontoFormateado
    {
        get
        {
            if (Monto is null) return "--";
            bool esEntrada = TipoMovimiento == "Deposito";
            return esEntrada ? $"+${Monto:N2}" : $"-${Monto:N2}";
        }
    }
}

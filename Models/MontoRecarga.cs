namespace BankIA.Models;

internal class MontoRecarga
{
    public int     MontoID     { get; set; }
    public int     ServicioID  { get; set; }
    public decimal Monto       { get; set; }
    public string? Descripcion { get; set; }
}

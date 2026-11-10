namespace BankIA.Models;

internal class Cajero
{
    public int    CajeroID     { get; set; }
    public int    BancoID      { get; set; }
    public string CodigoCajero { get; set; } = string.Empty;
    public string Ubicacion    { get; set; } = string.Empty;
    public string IPAddress    { get; set; } = string.Empty;
    public bool   Activo       { get; set; } = true;
}

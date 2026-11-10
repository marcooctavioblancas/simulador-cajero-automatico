namespace BankIA.Models;

internal class Tarjeta
{
    public int       TarjetaID        { get; set; }
    public int       CuentaID         { get; set; }
    public string    NumeroTarjeta    { get; set; } = string.Empty;
    public string    NIPHash          { get; set; } = string.Empty;
    public DateTime  FechaExpiracion  { get; set; }
    public string    TipoTarjeta      { get; set; } = "Debito";
    public int       IntentosFallidos { get; set; }
    public bool      Bloqueada        { get; set; }
    public DateTime? FechaBloqueo     { get; set; }
    public bool      Activa           { get; set; } = true;

    public string NumeroMascarado =>
        NumeroTarjeta.Length >= 4 ? $"•••• •••• •••• {NumeroTarjeta[^4..]}" : NumeroTarjeta;

    public bool EstaVigente => FechaExpiracion >= DateTime.Today;
}

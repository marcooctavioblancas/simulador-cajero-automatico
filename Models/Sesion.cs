namespace BankIA.Models;

internal class Sesion
{
    public int       SesionID    { get; set; }
    public int       TarjetaID   { get; set; }
    public int?      CajeroID    { get; set; }
    public DateTime  FechaInicio { get; set; }
    public DateTime? FechaFin    { get; set; }
    public string    Estado      { get; set; } = "Activa";
}

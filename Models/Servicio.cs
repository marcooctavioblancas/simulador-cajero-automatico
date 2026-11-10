namespace BankIA.Models;

public class Servicio
{
    public int     ServicioID    { get; set; }
    public string  Nombre        { get; set; } = string.Empty;
    public string  Icono         { get; set; } = string.Empty;
    public string  TipoServicio  { get; set; } = string.Empty; // "Servicio" | "Recarga"
    public string? Instrucciones { get; set; }
    public bool    Activo        { get; set; } = true;

    public bool EsRecarga => TipoServicio == "Recarga";
}

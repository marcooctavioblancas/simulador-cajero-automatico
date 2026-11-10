namespace BankIA.Models;

internal class Cliente
{
    public int      ClienteID      { get; set; }
    public string   NombreCompleto { get; set; } = string.Empty;
    public string   Identificacion { get; set; } = string.Empty;
    public string?  Telefono       { get; set; }
    public string?  Direccion      { get; set; }
    public string?  Email          { get; set; }
    public DateTime FechaAlta      { get; set; }
    public bool     Activo         { get; set; } = true;
}

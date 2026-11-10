namespace BankIA.Models;

internal class Banco
{
    public int    BancoID     { get; set; }
    public string CodigoBanco { get; set; } = string.Empty;
    public string NombreBanco { get; set; } = string.Empty;
    public string Pais        { get; set; } = "México";
    public bool   Activo      { get; set; } = true;
}

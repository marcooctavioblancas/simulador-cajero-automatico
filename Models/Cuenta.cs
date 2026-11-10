namespace BankIA.Models;

internal class Cuenta
{
    public int      CuentaID        { get; set; }
    public int      ClienteID       { get; set; }
    public int      BancoID         { get; set; }
    public string   NumeroCuenta    { get; set; } = string.Empty;
    public string   TipoCuenta      { get; set; } = "Debito";
    public decimal  SaldoDisponible { get; set; }
    public decimal  SaldoContable   { get; set; }
    public string   Estado          { get; set; } = "Activa";
    public DateTime FechaCreacion   { get; set; }
    public bool     Activo              { get; set; } = true;
    public decimal  LimiteRetiroDiario  { get; set; } = 10000m;

    public string NumeroCuentaMascarado =>
        NumeroCuenta.Length >= 4 ? $"•••• {NumeroCuenta[^4..]}" : NumeroCuenta;
}

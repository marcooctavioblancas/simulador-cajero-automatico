namespace BankIA.Models;

internal class InventarioBilletes
{
    public int CajeroID     { get; set; }
    public int Denominacion { get; set; }  // 50 | 100 | 200 | 500 | 1000
    public int Cantidad     { get; set; }
}

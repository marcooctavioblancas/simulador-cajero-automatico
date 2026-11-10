namespace BankIA.Config;

internal static class AppConfig
{
    public const string ConnectionString =
        @"Server=localhost;Database=BankIA;Integrated Security=True;TrustServerCertificate=True;";

    public const string ATMCodigo            = "ATM-001";
    public const int    MaxIntentosFallidos  = 3;
    public const int    TimeoutSesionSegundos = 30;

    // ── Interoperabilidad multi-banco ─────────────────────────────────────
    // Nombre de la BD externa tal como aparece en el mismo servidor SQL
    public const string NombreBDExterna       = "Banco2";
    public const decimal ComisionInterbancaria = 15m;   // MXN por operación interbancaria
    public const decimal ComisionSinTarjeta    = 35m;   // MXN por retiro sin tarjeta física
    public const int    TimeoutBancoExterno    = 3;     // segundos para detectar disponibilidad
}

using Microsoft.Data.SqlClient;
using BankIA.Config;

namespace BankIA.DAL;

internal static class DatabaseHelper
{
    public static SqlConnection GetConnection()
        => new(AppConfig.ConnectionString);

    public static bool TestConnection(out string error)
    {
        try
        {
            using var conn = GetConnection();
            conn.Open();
            error = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    /// <summary>Genera referencia unica tipo BIA-20260605-12345</summary>
    public static string GenerarReferencia()
    {
        int sufijo = Random.Shared.Next(10000, 99999);
        return $"BIA-{DateTime.Now:yyyyMMdd}-{sufijo:D5}";
    }
}

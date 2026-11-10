using Microsoft.Data.SqlClient;
using BankIA.Config;

namespace BankIA.DAL;

internal static class SesionDAL
{
    public static int Abrir(int tarjetaID, int? cajeroID)
    {
        // OUTPUT no funciona con triggers de replicación — usar SCOPE_IDENTITY()
        const string sql = @"
            INSERT INTO Sesiones (TarjetaID, CajeroID, Estado)
            VALUES (@TID, @CID, 'Activa');
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TID", tarjetaID);
        cmd.Parameters.AddWithValue("@CID", (object?)cajeroID ?? DBNull.Value);
        return (int)cmd.ExecuteScalar()!;
    }

    public static void Cerrar(int sesionID)
    {
        if (sesionID <= 0) return; // sesión externa — no registrada en BankIA
        const string sql = @"
            UPDATE Sesiones
            SET FechaFin = GETDATE(), Estado = 'Cerrada'
            WHERE SesionID = @SID";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@SID", sesionID);
        cmd.ExecuteNonQuery();
    }

    public static void Expirar(int sesionID)
    {
        if (sesionID <= 0) return; // sesión externa — no registrada en BankIA
        const string sql = @"
            UPDATE Sesiones
            SET FechaFin = GETDATE(), Estado = 'Expirada'
            WHERE SesionID = @SID";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@SID", sesionID);
        cmd.ExecuteNonQuery();
    }

    public static void ExpirarInactivas(int timeoutSegundos = 90)
    {
        const string sql = @"
            UPDATE Sesiones
            SET Estado = 'Expirada', FechaFin = GETDATE()
            WHERE Estado = 'Activa'
              AND DATEDIFF(SECOND, FechaInicio, GETDATE()) > @Timeout";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Timeout", timeoutSegundos);
        cmd.ExecuteNonQuery();
    }
}

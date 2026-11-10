using Microsoft.Data.SqlClient;
using BankIA.Models;
using BankIA.Config;
using BankIA.Helpers;

namespace BankIA.DAL;

internal static class TarjetaDAL
{
    public static Tarjeta? ObtenerPorNumero(string numeroTarjeta)
    {
        const string sql = @"
            SELECT TarjetaID, CuentaID, NumeroTarjeta, NIPHash,
                   FechaExpiracion, TipoTarjeta, IntentosFallidos,
                   Bloqueada, FechaBloqueo, Activa
            FROM Tarjetas
            WHERE NumeroTarjeta = @NumeroTarjeta";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    public static Tarjeta? ObtenerPorID(int tarjetaID)
    {
        const string sql = @"
            SELECT TarjetaID, CuentaID, NumeroTarjeta, NIPHash,
                   FechaExpiracion, TipoTarjeta, IntentosFallidos,
                   Bloqueada, FechaBloqueo, Activa
            FROM Tarjetas WHERE TarjetaID = @TarjetaID";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TarjetaID", tarjetaID);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    /// <summary>
    /// Verifica el NIP. Si es correcto resetea intentos.
    /// Si falla, incrementa y bloquea al llegar al maximo.
    /// Retorna true si el NIP es correcto.
    /// </summary>
    public static bool VerificarNIP(int tarjetaID, string nipIngresado, out string mensaje)
    {
        var tarjeta = ObtenerPorID(tarjetaID);
        if (tarjeta is null)       { mensaje = "Tarjeta no encontrada."; return false; }
        if (!tarjeta.Activa)       { mensaje = "Tarjeta inactiva.";      return false; }
        if (tarjeta.Bloqueada)     { mensaje = "Tarjeta bloqueada.";     return false; }
        if (!tarjeta.EstaVigente)  { mensaje = "Tarjeta vencida.";       return false; }

        if (SecurityHelper.VerificarNIP(nipIngresado, tarjeta.NIPHash))
        {
            ResetearIntentos(tarjetaID);
            mensaje = "OK";
            return true;
        }

        int nuevos = tarjeta.IntentosFallidos + 1;
        if (nuevos >= AppConfig.MaxIntentosFallidos)
        {
            BloquearTarjeta(tarjetaID, nuevos);
            mensaje = "Tarjeta bloqueada por exceso de intentos fallidos.";
        }
        else
        {
            ActualizarIntentos(tarjetaID, nuevos);
            int restantes = AppConfig.MaxIntentosFallidos - nuevos;
            mensaje = $"NIP incorrecto. Le quedan {restantes} intento(s).";
        }
        return false;
    }

    public static bool CambiarNIP(int tarjetaID, string nipNuevo, int? cajeroID)
    {
        string nuevoHash = SecurityHelper.HashNIP(nipNuevo);
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            var cmd1 = new SqlCommand(
                "UPDATE Tarjetas SET NIPHash = @Hash WHERE TarjetaID = @ID", conn, tx);
            cmd1.Parameters.AddWithValue("@Hash", nuevoHash);
            cmd1.Parameters.AddWithValue("@ID", tarjetaID);
            cmd1.ExecuteNonQuery();

            var cmd2 = new SqlCommand(
                "INSERT INTO CambiosNIP (TarjetaID, CajeroID, Exitoso) VALUES (@TID, @CID, 1)", conn, tx);
            cmd2.Parameters.AddWithValue("@TID", tarjetaID);
            cmd2.Parameters.AddWithValue("@CID", (object?)cajeroID ?? DBNull.Value);
            cmd2.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch { tx.Rollback(); return false; }
    }

    private static void ResetearIntentos(int tarjetaID)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE Tarjetas SET IntentosFallidos = 0 WHERE TarjetaID = @ID", conn);
        cmd.Parameters.AddWithValue("@ID", tarjetaID);
        cmd.ExecuteNonQuery();
    }

    private static void ActualizarIntentos(int tarjetaID, int intentos)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE Tarjetas SET IntentosFallidos = @N WHERE TarjetaID = @ID", conn);
        cmd.Parameters.AddWithValue("@N", intentos);
        cmd.Parameters.AddWithValue("@ID", tarjetaID);
        cmd.ExecuteNonQuery();
    }

    private static void BloquearTarjeta(int tarjetaID, int intentos)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            "UPDATE Tarjetas SET Bloqueada = 1, FechaBloqueo = GETDATE(), IntentosFallidos = @N WHERE TarjetaID = @ID", conn);
        cmd.Parameters.AddWithValue("@N", intentos);
        cmd.Parameters.AddWithValue("@ID", tarjetaID);
        cmd.ExecuteNonQuery();
    }

    private static Tarjeta Mapear(SqlDataReader r) => new()
    {
        TarjetaID        = r.GetInt32(0),
        CuentaID         = r.GetInt32(1),
        NumeroTarjeta    = r.GetString(2),
        NIPHash          = r.GetString(3),
        FechaExpiracion  = r.GetDateTime(4),
        TipoTarjeta      = r.GetString(5),
        IntentosFallidos = r.GetInt32(6),
        Bloqueada        = r.GetBoolean(7),
        FechaBloqueo     = r.IsDBNull(8) ? null : r.GetDateTime(8),
        Activa           = r.GetBoolean(9)
    };

    // ── Interoperabilidad: búsqueda en BD externa ─────────────────────────

    /// <summary>
    /// Busca primero en BankIA, luego en Banco2.
    /// Retorna la tarjeta y si es de banco externo.
    /// </summary>
    public static (Tarjeta? tarjeta, bool esExterno) ObtenerGlobal(string numeroTarjeta)
    {
        var local = ObtenerPorNumero(numeroTarjeta);
        if (local is not null) return (local, false);

        try
        {
            var ext = ObtenerDeExterno(numeroTarjeta);
            return (ext, ext is not null);
        }
        catch
        {
            return (null, false);
        }
    }

    /// <summary>
    /// Lee una tarjeta de Banco2.dbo.Tarjetas.
    /// NIPHash viene como varbinary — se convierte a hex string para usar el mismo SecurityHelper.
    /// </summary>
    private static Tarjeta? ObtenerDeExterno(string numeroTarjeta)
    {
        // VARCHAR(128) para soportar SHA2_512 (64 bytes = 128 hex chars)
        string sql = $@"
            SELECT t.TarjetaID, t.CuentaID, t.NumeroTarjeta,
                   CONVERT(VARCHAR(128), t.NIPHash, 2) AS NIPHashHex,
                   t.FechaExpiracion, t.Bloqueada
            FROM [{AppConfig.NombreBDExterna}].dbo.Tarjetas t
            WHERE t.NumeroTarjeta = @NumeroTarjeta";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.CommandTimeout = AppConfig.TimeoutBancoExterno;
        cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new Tarjeta
        {
            TarjetaID       = r.GetInt32(0),
            CuentaID        = r.GetInt32(1),
            NumeroTarjeta   = r.GetString(2),
            NIPHash         = r.GetString(3),  // ya es hex por el CONVERT
            FechaExpiracion = r.GetDateTime(4),
            Bloqueada       = r.GetBoolean(5),
            TipoTarjeta     = "Debito",
            Activa          = true
        };
    }

    /// <summary>Bloquea la tarjeta en Banco2 tras agotar intentos.</summary>
    public static void BloquearExterno(int tarjetaID)
    {
        string sql = $"UPDATE [{AppConfig.NombreBDExterna}].dbo.Tarjetas SET Bloqueada = 1 WHERE TarjetaID = @ID";
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ID", tarjetaID);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Obtiene cuenta y nombre completo del cliente desde Banco2.
    /// Mapea a la estructura Cuenta/Cliente de BankIA.
    /// </summary>
    public static (Models.Cuenta? cuenta, Models.Cliente? cliente) ObtenerCuentaClienteExterno(int cuentaID)
    {
        string sql = $@"
            SELECT c.CuentaID, c.ClienteID, c.NumeroCuenta, c.Saldo,
                   cl.Nombre + ' ' + cl.ApellidoP + ISNULL(' ' + cl.ApellidoM, '') AS NombreCompleto
            FROM [{AppConfig.NombreBDExterna}].dbo.Cuentas c
            JOIN [{AppConfig.NombreBDExterna}].dbo.Clientes cl ON c.ClienteID = cl.ClienteID
            WHERE c.CuentaID = @CuentaID AND c.Activa = 1";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CuentaID", cuentaID);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return (null, null);

        var cuenta = new Models.Cuenta
        {
            CuentaID        = r.GetInt32(0),
            ClienteID       = r.GetInt32(1),
            NumeroCuenta    = r.GetString(2),
            SaldoDisponible = r.GetDecimal(3),
            SaldoContable   = r.GetDecimal(3),
            TipoCuenta      = "Debito",
            Estado          = "Activa",
            BancoID         = 2,
            Activo          = true
        };
        var cliente = new Models.Cliente
        {
            ClienteID      = r.GetInt32(1),
            NombreCompleto = r.GetString(4)
        };
        return (cuenta, cliente);
    }
}

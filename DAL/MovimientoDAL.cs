using Microsoft.Data.SqlClient;
using BankIA.Models;

namespace BankIA.DAL;

internal static class MovimientoDAL
{
    public static List<Movimiento> ObtenerUltimos(int cuentaID, int cantidad = 10)
    {
        const string sql = @"
            SELECT TOP (@N) MovimientoID, CuentaID, TarjetaID, CajeroID,
                   TipoMovimiento, Monto, SaldoAnterior, SaldoPosterior,
                   CuentaDestinoID, Referencia, Descripcion, Estado,
                   EsInterbancario, FechaMovimiento
            FROM Movimientos
            WHERE CuentaID = @CuentaID
            ORDER BY FechaMovimiento DESC";

        var lista = new List<Movimiento>();
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@N", cantidad);
        cmd.Parameters.AddWithValue("@CuentaID", cuentaID);
        using var r = cmd.ExecuteReader();
        while (r.Read()) lista.Add(Mapear(r));
        return lista;
    }

    public static bool Retiro(int cuentaID, int tarjetaID, int cajeroID,
        decimal monto, out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error = string.Empty;

        var cuenta = CuentaDAL.ObtenerPorID(cuentaID);
        if (cuenta is null)                    { error = "Cuenta no encontrada.";  return false; }
        if (cuenta.SaldoDisponible < monto)    { error = "Saldo insuficiente.";    return false; }
        if (cuenta.Estado != "Activa")         { error = "Cuenta no disponible.";  return false; }

        decimal retirosHoy = RetirosDiarios(cuentaID);
        if (retirosHoy + monto > cuenta.LimiteRetiroDiario)
        {
            decimal disponible = cuenta.LimiteRetiroDiario - retirosHoy;
            error = $"Excede límite diario. Disponible hoy: ${disponible:N2}";
            return false;
        }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            decimal antes = cuenta.SaldoDisponible;
            InsertarMovimiento(conn, tx, cuentaID, tarjetaID, cajeroID,
                "Retiro", monto, antes, antes - monto, null, referencia, "Retiro de efectivo");

                var cmdSaldo = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible - @M WHERE CuentaID = @ID",
                conn, tx);
            cmdSaldo.Parameters.AddWithValue("@M",  monto);
            cmdSaldo.Parameters.AddWithValue("@ID", cuentaID);
            cmdSaldo.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    public static bool Deposito(int cuentaID, int? tarjetaID, int cajeroID,
        decimal monto, out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error = string.Empty;

        var cuenta = CuentaDAL.ObtenerPorID(cuentaID);
        if (cuenta is null) { error = "Cuenta no encontrada."; return false; }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            decimal antes = cuenta.SaldoDisponible;
            InsertarMovimiento(conn, tx, cuentaID, tarjetaID, cajeroID,
                "Deposito", monto, antes, antes + monto, null, referencia, "Deposito de efectivo");

            var cmdSaldo = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible + @M, SaldoContable = SaldoContable + @M WHERE CuentaID = @ID",
                conn, tx);
            cmdSaldo.Parameters.AddWithValue("@M",  monto);
            cmdSaldo.Parameters.AddWithValue("@ID", cuentaID);
            cmdSaldo.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    public static bool Transferencia(int cuentaOrigenID, int cuentaDestinoID,
        int tarjetaID, int cajeroID, decimal monto, out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error = string.Empty;

        var origen  = CuentaDAL.ObtenerPorID(cuentaOrigenID);
        var destino = CuentaDAL.ObtenerPorID(cuentaDestinoID);
        if (origen is null)                    { error = "Cuenta origen no encontrada.";  return false; }
        if (destino is null)                   { error = "Cuenta destino no encontrada."; return false; }
        if (origen.SaldoDisponible < monto)    { error = "Saldo insuficiente.";           return false; }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            InsertarMovimiento(conn, tx, cuentaOrigenID, tarjetaID, cajeroID,
                "Transferencia", monto, origen.SaldoDisponible, origen.SaldoDisponible - monto,
                cuentaDestinoID, referencia, "Transferencia enviada");

            var cmdOrigen = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible - @M WHERE CuentaID = @ID",
                conn, tx);
            cmdOrigen.Parameters.AddWithValue("@M",  monto);
            cmdOrigen.Parameters.AddWithValue("@ID", cuentaOrigenID);
            cmdOrigen.ExecuteNonQuery();

            var cmdDestino = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible + @M, SaldoContable = SaldoContable + @M WHERE CuentaID = @ID",
                conn, tx);
            cmdDestino.Parameters.AddWithValue("@M",  monto);
            cmdDestino.Parameters.AddWithValue("@ID", cuentaDestinoID);
            cmdDestino.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    public static bool Donacion(int cuentaID, int tarjetaID, int cajeroID,
        decimal monto, out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error = string.Empty;

        var cuenta = CuentaDAL.ObtenerPorID(cuentaID);
        if (cuenta is null || cuenta.SaldoDisponible < monto)
        {
            error = "Saldo insuficiente para donación.";
            return false;
        }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            decimal antes = cuenta.SaldoDisponible;
            InsertarMovimiento(conn, tx, cuentaID, tarjetaID, cajeroID,
                "Donacion", monto, antes, antes - monto, null,
                referencia, "Donación Cruz Roja Mexicana");

            var cmdSaldo = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible - @M WHERE CuentaID = @ID",
                conn, tx);
            cmdSaldo.Parameters.AddWithValue("@M",  monto);
            cmdSaldo.Parameters.AddWithValue("@ID", cuentaID);
            cmdSaldo.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    /// <summary>
    /// Retiro sin tarjeta física. Aplica comisión de $35 MXN.
    /// El NIP se verifica contra la tarjeta asociada a la cuenta.
    /// </summary>
    public static bool RetiroSinTarjeta(int cuentaID, decimal monto,
        out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error      = string.Empty;
        decimal comision = Config.AppConfig.ComisionSinTarjeta;
        decimal total    = monto + comision;

        var cuenta = CuentaDAL.ObtenerPorID(cuentaID);
        if (cuenta is null)               { error = "Cuenta no encontrada.";  return false; }
        if (cuenta.SaldoDisponible < total){ error = $"Saldo insuficiente. Necesita ${total:N2} (monto + comisión ${comision:N2})."; return false; }
        if (cuenta.Estado != "Activa")    { error = "Cuenta no disponible.";  return false; }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            decimal antes = cuenta.SaldoDisponible;
            InsertarMovimiento(conn, tx, cuentaID, null, Helpers.SessionManager.CajeroID,
                "RetiroSinTarjeta", total, antes, antes - total, null,
                referencia, $"Retiro sin tarjeta (incl. comisión ${comision:N2})");

            var cmdSaldo = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible - @M WHERE CuentaID = @ID",
                conn, tx);
            cmdSaldo.Parameters.AddWithValue("@M",  total);
            cmdSaldo.Parameters.AddWithValue("@ID", cuentaID);
            cmdSaldo.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    public static decimal RetirosDiarios(int cuentaID)
    {
        const string sql = @"
            SELECT ISNULL(SUM(Monto), 0)
            FROM Movimientos
            WHERE CuentaID = @CID
              AND TipoMovimiento = 'Retiro'
              AND Estado = 'Exitoso'
              AND CAST(FechaMovimiento AS DATE) = CAST(GETDATE() AS DATE)";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CID", cuentaID);
        return (decimal)cmd.ExecuteScalar()!;
    }

    public static bool PagoServicio(int cuentaID, int tarjetaID, int cajeroID,
        decimal monto, string nombreServicio, string referenciaPago,
        out string refMovimiento, out string error)
    {
        refMovimiento = DatabaseHelper.GenerarReferencia();
        error = string.Empty;

        var cuenta = CuentaDAL.ObtenerPorID(cuentaID);
        if (cuenta is null)                 { error = "Cuenta no encontrada.";  return false; }
        if (cuenta.SaldoDisponible < monto) { error = "Saldo insuficiente.";    return false; }
        if (cuenta.Estado != "Activa")      { error = "Cuenta no disponible.";  return false; }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            decimal antes = cuenta.SaldoDisponible;
            InsertarMovimiento(conn, tx, cuentaID, tarjetaID, cajeroID,
                "PagoServicio", monto, antes, antes - monto, null,
                refMovimiento, $"Pago {nombreServicio} · Ref {referenciaPago}");

            var cmdSaldo = new SqlCommand(
                "UPDATE Cuentas SET SaldoDisponible = SaldoDisponible - @M WHERE CuentaID = @ID",
                conn, tx);
            cmdSaldo.Parameters.AddWithValue("@M",  monto);
            cmdSaldo.Parameters.AddWithValue("@ID", cuentaID);
            cmdSaldo.ExecuteNonQuery();

            tx.Commit();
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    // ── Operaciones para tarjetas de banco externo ─────────────────────────

    /// <summary>
    /// Retiro para cliente de banco externo.
    /// Debita (monto + comisión) de Banco2.Cuentas y registra en MensajesInterbancarios.
    /// </summary>
    public static bool RetiroExterno(int cuentaExternaID, decimal monto,
        out string referencia, out string error)
    {
        referencia = DatabaseHelper.GenerarReferencia();
        error      = string.Empty;
        decimal comision = Config.AppConfig.ComisionInterbancaria;
        decimal total    = monto + comision;

        // Verificar saldo en banco externo
        decimal saldo = ObtenerSaldoExterno(cuentaExternaID);
        if (saldo < total)
        {
            error = $"Saldo insuficiente en su banco. Necesita ${total:N2} (monto + comisión ${comision:N2}).";
            return false;
        }

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            // Debitar del banco externo
            var cmdDebit = new SqlCommand(
                $"UPDATE [{Config.AppConfig.NombreBDExterna}].dbo.Cuentas SET Saldo = Saldo - @Total WHERE CuentaID = @CID",
                conn, tx);
            cmdDebit.Parameters.AddWithValue("@Total", total);
            cmdDebit.Parameters.AddWithValue("@CID",   cuentaExternaID);
            cmdDebit.ExecuteNonQuery();

            // Registrar en BankIA.MensajesInterbancarios
            var cmdMsg = new SqlCommand(@"
                INSERT INTO MensajesInterbancarios
                    (BancoOrigenID, BancoDestinoID, TipoMensaje, Monto, Estado, FechaProcesado, Respuesta)
                VALUES (2, 1, 'Retiro', @Monto, 'Aprobado', GETDATE(), @Ref)", conn, tx);
            cmdMsg.Parameters.AddWithValue("@Monto", monto);
            cmdMsg.Parameters.AddWithValue("@Ref",   referencia);
            cmdMsg.ExecuteNonQuery();

            tx.Commit();

            // Actualizar caché de sesión
            var cuenta = Helpers.SessionManager.CuentaActual;
            if (cuenta is not null)
            {
                cuenta.SaldoDisponible -= total;
                cuenta.SaldoContable   -= total;
            }
            return true;
        }
        catch (Exception ex) { tx.Rollback(); error = ex.Message; return false; }
    }

    private static decimal ObtenerSaldoExterno(int cuentaID)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(
            $"SELECT Saldo FROM [{Config.AppConfig.NombreBDExterna}].dbo.Cuentas WHERE CuentaID = @CID",
            conn);
        cmd.Parameters.AddWithValue("@CID", cuentaID);
        return (decimal)(cmd.ExecuteScalar() ?? 0m);
    }

    // ---- Helpers privados ----

    private static void InsertarMovimiento(SqlConnection conn, SqlTransaction tx,
        int cuentaID, int? tarjetaID, int? cajeroID, string tipo,
        decimal monto, decimal antes, decimal despues,
        int? cuentaDestinoID, string referencia, string descripcion)
    {
        const string sql = @"
            INSERT INTO Movimientos
                (CuentaID, TarjetaID, CajeroID, TipoMovimiento, Monto,
                 SaldoAnterior, SaldoPosterior, CuentaDestinoID,
                 Referencia, Descripcion, Estado)
            VALUES
                (@CID, @TID, @CAJID, @Tipo, @Monto,
                 @Antes, @Despues, @CDestino,
                 @Ref, @Desc, 'Exitoso')";

        var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@CID",     cuentaID);
        cmd.Parameters.AddWithValue("@TID",     (object?)tarjetaID    ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@CAJID",   (object?)cajeroID     ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Tipo",    tipo);
        cmd.Parameters.AddWithValue("@Monto",   monto);
        cmd.Parameters.AddWithValue("@Antes",   antes);
        cmd.Parameters.AddWithValue("@Despues", despues);
        cmd.Parameters.AddWithValue("@CDestino",(object?)cuentaDestinoID ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Ref",     referencia);
        cmd.Parameters.AddWithValue("@Desc",    descripcion);
        cmd.ExecuteNonQuery();
    }

    private static Movimiento Mapear(SqlDataReader r) => new()
    {
        MovimientoID    = r.GetInt32(0),
        CuentaID        = r.GetInt32(1),
        TarjetaID       = r.IsDBNull(2)  ? null : r.GetInt32(2),
        CajeroID        = r.IsDBNull(3)  ? null : r.GetInt32(3),
        TipoMovimiento  = r.GetString(4),
        Monto           = r.IsDBNull(5)  ? null : r.GetDecimal(5),
        SaldoAnterior   = r.GetDecimal(6),
        SaldoPosterior  = r.GetDecimal(7),
        CuentaDestinoID = r.IsDBNull(8)  ? null : r.GetInt32(8),
        Referencia      = r.GetString(9),
        Descripcion     = r.IsDBNull(10) ? null : r.GetString(10),
        Estado          = r.GetString(11),
        EsInterbancario = r.GetBoolean(12),
        FechaMovimiento = r.GetDateTime(13)
    };
}

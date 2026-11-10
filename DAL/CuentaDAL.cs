using Microsoft.Data.SqlClient;
using BankIA.Models;

namespace BankIA.DAL;

internal static class CuentaDAL
{
    public static Cuenta? ObtenerPorID(int cuentaID)
    {
        const string sql = @"
            SELECT CuentaID, ClienteID, BancoID, NumeroCuenta, TipoCuenta,
                   SaldoDisponible, SaldoContable, Estado, FechaCreacion, Activo,
                   LimiteRetiroDiario
            FROM Cuentas WHERE CuentaID = @CuentaID";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CuentaID", cuentaID);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    public static Cuenta? ObtenerPorNumero(string numeroCuenta)
    {
        const string sql = @"
            SELECT CuentaID, ClienteID, BancoID, NumeroCuenta, TipoCuenta,
                   SaldoDisponible, SaldoContable, Estado, FechaCreacion, Activo,
                   LimiteRetiroDiario
            FROM Cuentas WHERE NumeroCuenta = @Numero";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Numero", numeroCuenta);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    public static Cliente? ObtenerCliente(int clienteID)
    {
        const string sql = @"
            SELECT ClienteID, NombreCompleto, Identificacion, Telefono,
                   Direccion, Email, FechaAlta, Activo
            FROM Clientes WHERE ClienteID = @ID";

        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ID", clienteID);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return new Cliente
        {
            ClienteID      = r.GetInt32(0),
            NombreCompleto = r.GetString(1),
            Identificacion = r.GetString(2),
            Telefono       = r.IsDBNull(3) ? null : r.GetString(3),
            Direccion      = r.IsDBNull(4) ? null : r.GetString(4),
            Email          = r.IsDBNull(5) ? null : r.GetString(5),
            FechaAlta      = r.GetDateTime(6),
            Activo         = r.GetBoolean(7)
        };
    }

    private static Cuenta Mapear(SqlDataReader r) => new()
    {
        CuentaID           = r.GetInt32(0),
        ClienteID          = r.GetInt32(1),
        BancoID            = r.GetInt32(2),
        NumeroCuenta       = r.GetString(3),
        TipoCuenta         = r.GetString(4),
        SaldoDisponible    = r.GetDecimal(5),
        SaldoContable      = r.GetDecimal(6),
        Estado             = r.GetString(7),
        FechaCreacion      = r.GetDateTime(8),
        Activo             = r.GetBoolean(9),
        LimiteRetiroDiario = r.GetDecimal(10)
    };
}

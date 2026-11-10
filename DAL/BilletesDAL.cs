using Microsoft.Data.SqlClient;

namespace BankIA.DAL;

internal static class BilletesDAL
{
    private static readonly int[] Denominaciones = { 1000, 500, 200, 100, 50 };

    /// <summary>Lee el inventario actual del cajero.</summary>
    public static Dictionary<int, int> ObtenerInventario(int cajeroID)
    {
        const string sql = @"
            SELECT Denominacion, Cantidad
            FROM InventarioBilletes
            WHERE CajeroID = @CID
            ORDER BY Denominacion DESC";

        var resultado = new Dictionary<int, int>();
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@CID", cajeroID);
        using var r = cmd.ExecuteReader();
        while (r.Read()) resultado[r.GetInt32(0)] = r.GetInt32(1);
        return resultado;
    }

    /// <summary>
    /// Calcula el desglose óptimo (greedy: billetes más grandes primero).
    /// Retorna diccionario vacío si el monto no se puede cubrir con el inventario disponible.
    /// </summary>
    public static Dictionary<int, int> CalcularDesglose(decimal monto, Dictionary<int, int> inventario)
    {
        var desglose  = new Dictionary<int, int>();
        int restante  = (int)monto;

        foreach (int d in Denominaciones)
        {
            if (restante <= 0) break;
            int disponibles = inventario.GetValueOrDefault(d, 0);
            int usar        = Math.Min(restante / d, disponibles);
            if (usar > 0)
            {
                desglose[d] = usar;
                restante   -= usar * d;
            }
        }

        return restante == 0 ? desglose : new Dictionary<int, int>();
    }

    /// <summary>Descuenta los billetes del inventario. Llamar dentro de una transacción existente.</summary>
    public static void DescontarBilletes(int cajeroID, Dictionary<int, int> desglose,
        SqlConnection conn, SqlTransaction tx)
    {
        foreach (var (denom, cantidad) in desglose)
        {
            var cmd = new SqlCommand(@"
                UPDATE InventarioBilletes
                SET Cantidad = Cantidad - @Cant
                WHERE CajeroID = @CID AND Denominacion = @Denom",
                conn, tx);
            cmd.Parameters.AddWithValue("@Cant",  cantidad);
            cmd.Parameters.AddWithValue("@CID",   cajeroID);
            cmd.Parameters.AddWithValue("@Denom", denom);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Descuenta billetes en una conexión propia (fuera de transacción mayor).</summary>
    public static void DescontarBilletes(int cajeroID, Dictionary<int, int> desglose)
    {
        if (desglose.Count == 0) return;
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            DescontarBilletes(cajeroID, desglose, conn, tx);
            tx.Commit();
        }
        catch { tx.Rollback(); }
    }

    /// <summary>Formatea el desglose para mostrar en pantalla.</summary>
    public static string FormatearDesglose(Dictionary<int, int> desglose)
    {
        if (desglose.Count == 0)
            return "Sin billetes disponibles para este monto.";

        var partes = desglose
            .OrderByDescending(kv => kv.Key)
            .Select(kv => $"{kv.Value}×${kv.Key:N0}");
        return string.Join("  +  ", partes);
    }
}

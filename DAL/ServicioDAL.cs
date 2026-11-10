using Microsoft.Data.SqlClient;
using BankIA.Models;

namespace BankIA.DAL;

internal static class ServicioDAL
{
    public static List<Servicio> ObtenerActivos()
    {
        const string sql = @"
            SELECT ServicioID, Nombre, Icono, TipoServicio, Instrucciones, Activo
            FROM Servicios
            WHERE Activo = 1
            ORDER BY TipoServicio DESC, Nombre"; // 'S' > 'R': Servicios primero, Recargas al final

        var lista = new List<Servicio>();
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            lista.Add(new Servicio
            {
                ServicioID    = r.GetInt32(0),
                Nombre        = r.GetString(1),
                Icono         = r.GetString(2),
                TipoServicio  = r.GetString(3),
                Instrucciones = r.IsDBNull(4) ? null : r.GetString(4),
                Activo        = r.GetBoolean(5)
            });
        return lista;
    }

    public static List<MontoRecarga> ObtenerMontos(int servicioID)
    {
        const string sql = @"
            SELECT MontoID, ServicioID, Monto, Descripcion
            FROM MontosRecarga
            WHERE ServicioID = @ID
            ORDER BY Monto";

        var lista = new List<MontoRecarga>();
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ID", servicioID);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            lista.Add(new MontoRecarga
            {
                MontoID    = r.GetInt32(0),
                ServicioID = r.GetInt32(1),
                Monto      = r.GetDecimal(2),
                Descripcion= r.IsDBNull(3) ? null : r.GetString(3)
            });
        return lista;
    }
}

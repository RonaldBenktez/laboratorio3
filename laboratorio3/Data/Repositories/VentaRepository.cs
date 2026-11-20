using Microsoft.Data.SqlClient;
using RepoDb;
using laboratorio3.Models;

namespace laboratorio3.Data.Repositories
{
    public class VentaRepository : BaseRepository<Venta>
    {
        public IEnumerable<Venta> GetByFecha(DateTime desde, DateTime hasta)
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<Venta>(v => v.Fecha >= desde && v.Fecha <= hasta).ToList();
        }

        public IEnumerable<dynamic> GetReporteVentas()
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.ExecuteQuery<dynamic>(@"
                SELECT p.Nombre AS Producto, p.Categoria,
                       SUM(dv.Cantidad) AS TotalVendido,
                       SUM(dv.Subtotal) AS MontoTotal,
                       MAX(v.Fecha) AS UltimaVenta
                FROM DetalleVentas dv
                INNER JOIN Productos p ON p.Id = dv.ProductoId
                INNER JOIN Ventas v ON v.Id = dv.VentaId
                GROUP BY p.Id, p.Nombre, p.Categoria
                ORDER BY TotalVendido DESC").ToList();
        }
    }
}

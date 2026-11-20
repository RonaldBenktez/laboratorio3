using MySql.Data.MySqlClient;
using RepoDb;
using laboratorio3.Models;

namespace laboratorio3.Data.Repositories
{
    public class DetalleVentaRepository : BaseRepository<DetalleVenta>
    {
        public IEnumerable<DetalleVenta> GetByVentaId(int ventaId)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Query<DetalleVenta>(d => d.VentaId == ventaId).ToList();
        }
    }
}

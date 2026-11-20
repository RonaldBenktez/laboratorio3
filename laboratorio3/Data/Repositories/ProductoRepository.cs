using Microsoft.Data.SqlClient;
using RepoDb;
using laboratorio3.Models;

namespace laboratorio3.Data.Repositories
{
    public class ProductoRepository : BaseRepository<Producto>
    {
        public IEnumerable<Producto> GetActivos()
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<Producto>(p => p.Activo == true).ToList();
        }

        public void ActualizarStock(int productoId, int nuevoStock)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.ExecuteNonQuery(
                "UPDATE Productos SET Stock = @Stock WHERE Id = @Id",
                new { Stock = nuevoStock, Id = productoId });
        }
    }
}

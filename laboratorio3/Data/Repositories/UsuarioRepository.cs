using MySql.Data.MySqlClient;
using RepoDb;
using laboratorio3.Models;

namespace laboratorio3.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public Usuario? GetByEmailAndHash(string email, string passwordHash)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Query<Usuario>(u => u.Email == email && u.PasswordHash == passwordHash && u.Activo == true)
                       .FirstOrDefault();
        }
    }
}

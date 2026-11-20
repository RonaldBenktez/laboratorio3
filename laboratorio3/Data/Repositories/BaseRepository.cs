using MySql.Data.MySqlClient;
using RepoDb;
using laboratorio3.Data.Interfaces;

namespace laboratorio3.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly string _connectionString;

        public BaseRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        public virtual IEnumerable<T> GetAll()
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.QueryAll<T>().ToList();
        }

        public virtual T? GetById(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Query<T>(id).FirstOrDefault();
        }

        public virtual int Insert(T entity)
        {
            using var conn = new MySqlConnection(_connectionString);
            return Convert.ToInt32(conn.Insert<T>(entity));
        }

        public virtual int Update(T entity)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Update<T>(entity);
        }

        public virtual int Delete(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Delete<T>(id);
        }
    }
}

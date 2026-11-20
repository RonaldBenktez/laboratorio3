using RepoDb;

namespace laboratorio3.Data
{
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; private set; } = string.Empty;

        public static void Initialize(string connectionString)
        {
            ConnectionString = connectionString;
            GlobalConfiguration.Setup().UseMySql();
        }
    }
}

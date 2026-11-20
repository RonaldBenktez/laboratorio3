using laboratorio3.Data;
using laboratorio3.Forms;

namespace laboratorio3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Cadena de conexión — se lee de appsettings.local.ini (excluido de git)
            string connectionString = ReadConnectionString();

            DatabaseConfig.Initialize(connectionString);

            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
                return;

            Application.Run(new MainForm());
        }

        private static string ReadConnectionString()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.local.ini");
            if (File.Exists(configPath))
            {
                foreach (var line in File.ReadAllLines(configPath))
                {
                    if (line.StartsWith("ConnectionString="))
                        return line["ConnectionString=".Length..].Trim();
                }
            }
            // Fallback sin contraseña (para demo sin credenciales en el repo)
            return "Server=localhost;Database=TiendaElectronica;Uid=root;Pwd=;";
        }
    }
}

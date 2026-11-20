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

            // Cadena de conexión — ajusta Server según tu instancia de SQL Server
            string connectionString =
                "Server=localhost;Database=TiendaElectronica;Trusted_Connection=True;TrustServerCertificate=True;";

            DatabaseConfig.Initialize(connectionString);

            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
                return;

            Application.Run(new MainForm());
        }
    }
}

using RepoDb.Attributes;

namespace laboratorio3.Models
{
    [Map("Usuarios")]
    public class Usuario
    {
        [Primary, Identity]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Vendedor";
        public bool Activo { get; set; } = true;
    }
}

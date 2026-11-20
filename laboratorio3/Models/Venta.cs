using RepoDb.Attributes;

namespace laboratorio3.Models
{
    [Map("Ventas")]
    public class Venta
    {
        [Primary, Identity]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Completada";
    }
}

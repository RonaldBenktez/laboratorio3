using RepoDb.Attributes;

namespace laboratorio3.Models
{
    [Map("Productos")]
    public class Producto
    {
        [Primary, Identity]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;
    }
}

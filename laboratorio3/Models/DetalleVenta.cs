using RepoDb.Attributes;

namespace laboratorio3.Models
{
    [Map("DetalleVentas")]
    public class DetalleVenta
    {
        [Primary, Identity]
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}

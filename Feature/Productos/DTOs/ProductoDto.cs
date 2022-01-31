using Facturacion.Feature.Movimientos;

namespace Facturacion.Feature.Productos.DTOs
{
    public class ProductoDto
    {
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
using Facturacion.Feature.Facturas;
using Facturacion.Feature.Movimientos;

namespace Facturacion.Feature.Productos
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public int PrecioUnitario { get; set; }
        public int Existencia { get; set; }
        public virtual List<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();
        public virtual List<Movimiento> Movimiento { get; set; } = new List<Movimiento>();
    }
}
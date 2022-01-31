using Facturacion.Feature.Productos;

namespace Facturacion.Feature.Movimientos
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public int ProductoId { get; set; }
        public string? Descripcion { get; set; }
        public string? Tipo { get; set; }
        public int Cantidad { get; set; }
        public int Total { get; set; }
        
        public Producto? Producto { get; set; }
        public virtual List<TipoMovimiento> TipoMovimiento { get; set; } = new List<TipoMovimiento>();
    }
}
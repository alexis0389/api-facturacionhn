using Facturacion.Feature.Clientes;

namespace Facturacion.Feature.Facturas
{
    public class Factura
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }
      
        public DateTime Fecha { get; set; }

        public decimal Descuento { get; set; }
        public decimal IVA { get; set; }
        public virtual List<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();
    }
}
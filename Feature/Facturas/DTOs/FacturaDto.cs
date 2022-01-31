namespace Facturacion.Feature.Facturas.DTOs
{
    public class FacturaDto
    {
        public int ClienteId { get; set; }
        public List<FacturaProductoDto> Productos { get; set; } = new List<FacturaProductoDto>();
    }
}
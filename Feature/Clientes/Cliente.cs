using System.ComponentModel.DataAnnotations;
using Facturacion.Feature.Facturas;

namespace Facturacion.Feature.Clientes
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public virtual List<Factura> Facturas { get; set; } = new List<Factura>();
    }
}
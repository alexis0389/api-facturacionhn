namespace Facturacion.Feature.Movimientos
{
    public class TipoMovimiento
    {
        public int Id { get; set; }
        public int MovimientoId { get; set; }
        public string? Entrada_Salida { get; set; }
        public Movimiento? Movimiento { get; set; }
        
    }
}
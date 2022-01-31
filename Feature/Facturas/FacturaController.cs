using Facturacion.Feature.Facturas.DTOs;
using Facturacion.Feature.Movimientos;
using Facturacion.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Feature.Facturas
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : Controller
    {
        private readonly FacturacionDbContext _context;
        public FacturaController(FacturacionDbContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> Get()
        {
            try
            {
                return await _context.Facturas.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> Get(int id)
        {
            try
            {
                var facturaid = await _context.Facturas.FindAsync(id);
                if (facturaid != null)
                {
                    return facturaid;
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> Post(FacturaDto facturaDto)
        {
            Factura factura = new Factura();
            factura.ClienteId = facturaDto.ClienteId;
            factura.Fecha = DateTime.Now.Date;

            var productosId = facturaDto.Productos.Select(p => p.ProductoId).ToList();
            var productos = _context.Productos
                                    .Where(p => productosId.Contains(p.Id))
                                    .ToList();

            foreach (var productoP in productos)
            {
                var detalle = new FacturaDetalle();
                detalle.Factura = factura;
                detalle.ProductoId = productoP.Id;
                detalle.PrecioUnitario = productoP.PrecioUnitario;
                detalle.Cantidad = facturaDto.Productos
                                             .Find(c => c.ProductoId == productoP.Id)?
                                             .Cantidad ?? 0;
                
                // Restar Existencia
                if (detalle.Cantidad > productoP.Existencia)
                {
                    return BadRequest("La cantidad requerida es mayor a la existencia");
                }
                
                productoP.Existencia -= detalle.Cantidad;
                
                var movimiento = new Movimiento();
                var tipoMovi = new TipoMovimiento();
                // Tabla tipo Movimiento
                tipoMovi.MovimientoId = movimiento.Id;
                tipoMovi.Entrada_Salida = "Salida";
            
                // Tabla Movimientos
                movimiento.Fecha = DateTime.Now.Date;
                movimiento.Hora = DateTime.Now;
                movimiento.ProductoId = productoP.Id;
                movimiento.Descripcion = "Venta";
                movimiento.Tipo = tipoMovi.Entrada_Salida;
                movimiento.Cantidad = detalle.Cantidad;
                movimiento.Total = productoP.Existencia;
                
                movimiento.TipoMovimiento.Add(tipoMovi);
                factura.FacturaDetalle.Add(detalle);
                productoP.Movimiento.Add(movimiento);
            }
        
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Facturas.Add(factura);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (OperationCanceledException)
            {
                await transaction.RollbackAsync();
            }
            return Content("Datos Creados Satisfactoriamente!");
        }
    }
}

    
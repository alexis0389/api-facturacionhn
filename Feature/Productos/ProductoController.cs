using Facturacion.Feature.Movimientos;
using Facturacion.Feature.Productos.DTOs;
using Facturacion.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Feature.Productos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly FacturacionDbContext _context;
        public ProductoController(FacturacionDbContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            try
            {
                return await _context.Productos.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            try
            {
                var productoid = await _context.Productos.FindAsync(id);
                if (productoid != null)
                {
                    return productoid;
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
        public async Task<ActionResult<Producto>> Post(ProductoDto productoDto)
        {
            Producto producto = new Producto();
            producto.Nombre = productoDto.Nombre;
            producto.Categoria = productoDto.Categoria;
            producto.PrecioUnitario = productoDto.Precio;
            producto.Existencia += productoDto.Cantidad; 

            var movimiento = new Movimiento();
            var tipoMovimiento = new TipoMovimiento();
            tipoMovimiento.MovimientoId = movimiento.Id;
            tipoMovimiento.Entrada_Salida = "Entrada";

            movimiento.Producto = producto;
            movimiento.Fecha = DateTime.Now.Date;
            movimiento.Hora = DateTime.Now;
            movimiento.Descripcion = "Compra";
            movimiento.Tipo = tipoMovimiento.Entrada_Salida;
            movimiento.Cantidad = productoDto.Cantidad;
            movimiento.Total = producto.Existencia;

            producto.Movimiento.Add(movimiento);
            movimiento.TipoMovimiento.Add(tipoMovimiento);

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
            }
            return Content("Datos Creados Satisfactoriamente!");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Producto producto)
        {
            if (producto.Id != id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Content("Datos Actualizados Satisfactoriamente!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Content("Datos Borrados Satisfactoriamente!");
        }

        private bool ProductoExist(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
using Facturacion.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Feature.Movimientos
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : Controller
    {
        private readonly FacturacionDbContext _context;
        public MovimientoController(FacturacionDbContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimiento>>> Get()
        {
            try
            {
                return await _context.Movimientos.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimiento>> Get(int id)
        {
            try
            {
                var movimientoId = await _context.Movimientos.FindAsync(id);
                if (movimientoId != null)
                {
                    return movimientoId;
                }
                else
                {
                    return BadRequest("Movimiento no encontrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
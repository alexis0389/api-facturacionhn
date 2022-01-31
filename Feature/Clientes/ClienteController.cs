using Facturacion.Feature.Clientes.DTOs;
using Facturacion.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Feature.Clientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly FacturacionDbContext _context;

        public ClienteController(FacturacionDbContext context)
        {
            this._context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            try
            {
                var clienteid = await _context.Clientes.FindAsync(id);
                if (clienteid != null)
                {
                    return clienteid;
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
        public async Task<ActionResult<Cliente>> Post(ClienteDto clienteDto)
        {
            Cliente cliente = new Cliente();
            cliente.Nombre = clienteDto.Nombre;
            cliente.Direccion = clienteDto.Direccion;
            cliente.Email = clienteDto.Email;

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Clientes.Add(cliente);
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
        public async Task<ActionResult> Put(int id, Cliente cliente)
        {
            if (cliente.Id != id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExist(id))
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
            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes == null)
                {
                    return NotFound();
                }

            _context.Clientes.Remove(clientes);
            await _context.SaveChangesAsync();

            return Content("Datos Borrados Satisfactoriamente!");
        }

        private bool ClienteExist(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
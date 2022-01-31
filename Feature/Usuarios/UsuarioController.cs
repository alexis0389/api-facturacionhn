using Facturacion.Services;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.Feature.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration? configuration;
        private readonly ITokenService? tokenService;

        public UsuarioController(IConfiguration configuration, ITokenService tokenServices)
        {
            this.configuration = configuration;
            this.tokenService = tokenServices;
        }


        [HttpPost]
        [Route("Sign-in")]
        public IActionResult Post(Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Solicitud de cliente invalida");

            if (usuario.Email == "Eduardo@gmail.com" && usuario.Contraseña == "12345678") {
                return Ok(tokenService?.BuildToken(
                    configuration["Jwt:AuthDemo:Key"],
                    configuration["Jwt:AuthDemo:ValidIssuer"],
                    usuario
                ));
            }else {
                return Unauthorized();
            }
        }
    }
}
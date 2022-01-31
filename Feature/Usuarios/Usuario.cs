using System.ComponentModel.DataAnnotations;

namespace Facturacion.Feature.Usuarios
{
    public class Usuario
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Contraseña { get; set; }
    }
}
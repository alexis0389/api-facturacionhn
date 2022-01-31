using Facturacion.Feature.Usuarios;

namespace Facturacion.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, Usuario usuario);
    }
}
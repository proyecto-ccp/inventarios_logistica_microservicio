
using Inventarios.Dominio.ObjetoValor;

namespace Inventarios.Dominio.Puertos.Integraciones
{
    public interface IServicioUsuariosApi
    {
        Task<TokenInfo> ValidarToken(string token);
    }
}

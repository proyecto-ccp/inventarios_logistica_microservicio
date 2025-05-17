using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Inventarios.Dominio.ObjetoValor
{
    [ExcludeFromCodeCoverage]
    public class TokenInfo
    {
        public string IdUsuario { get; set; }
        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}

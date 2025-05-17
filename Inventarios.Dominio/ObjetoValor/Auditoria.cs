
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Dominio.ObjetoValor
{
    [ExcludeFromCodeCoverage]
    public class Auditoria
    {
        public Guid IdUsuario { get; set; }
        public string Accion { get; set; }
        public string TablaAfectada { get; set; }
        public string IdRegistro { get; set; }
        public string Registro { get; set; }
    }
}

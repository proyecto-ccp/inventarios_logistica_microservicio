using Inventarios.Dominio.ObjetoValor;

namespace Inventarios.Dominio.Puertos.Integraciones
{
    public interface IServicioAuditoriaApi
    {
        Task RegistrarAuditoria(Auditoria auditoria);
    }
}

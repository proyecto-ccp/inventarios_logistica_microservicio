
using Inventarios.Dominio.Entidades;

namespace Inventarios.Dominio.Puertos.Repositorios
{
    public interface IInventarioRepositorio
    {
        Task<Inventario> CrearStock(Inventario input);
        Task<Inventario> ActualizarStock(Inventario input);
        Task<Inventario> ConsultarStock(int idProducto);
    }
}

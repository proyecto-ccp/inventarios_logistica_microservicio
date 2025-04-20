using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Repositorios;
using Inventarios.Infraestructura.Adaptadores.RepositorioGenerico;
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Infraestructura.Adaptadores.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class InventarioRepositorio : IInventarioRepositorio
    {
        private readonly IRepositorioBase<Inventario> _repositorioProducto;

        public InventarioRepositorio(IRepositorioBase<Inventario> repositorioProducto)
        {
            _repositorioProducto = repositorioProducto;
        }

        public async Task<Inventario> ActualizarStock(Inventario input)
        {
            return await _repositorioProducto.Actualizar(input);
        }

        public async Task<Inventario> ConsultarStock(int idProducto)
        {
            return await _repositorioProducto.BuscarPorCampos(t => t.IdProducto == idProducto);
        }

        public async Task<Inventario> CrearStock(Inventario input)
        {
            return await _repositorioProducto.Guardar(input);
        }

    }
}

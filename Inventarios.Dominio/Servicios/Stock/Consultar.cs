
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Repositorios;

namespace Inventarios.Dominio.Servicios.Stock
{
    public class Consultar(IInventarioRepositorio inventario)
    {
        private readonly IInventarioRepositorio _inventario = inventario;

        public async Task<Inventario> Ejecutar(int idProducto)
        {
            return await _inventario.ConsultarStock(idProducto) ?? throw new ArgumentException("El producto no existe en el inventario");
        }
    }
}

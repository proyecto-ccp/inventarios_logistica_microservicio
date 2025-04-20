using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Repositorios;

namespace Inventarios.Dominio.Servicios.Stock
{
    public class Retirar(IInventarioRepositorio inventario)
    {
        private readonly IInventarioRepositorio _inventario = inventario;
        private readonly Consultar consultarInventario = new (inventario);

        public async Task<Inventario> Ejecutar(Inventario input)
        {
            Inventario output;

            await ValidarCantidad(input.CantidadStock);
            var productoEnInventario = await consultarInventario.Ejecutar(input.IdProducto);
            await ValidarDisponible(productoEnInventario.CantidadStock, input.CantidadStock);
            productoEnInventario.CantidadStock -= input.CantidadStock;
            productoEnInventario.FechaModificacion = DateTime.Now;
            output = await _inventario.ActualizarStock(productoEnInventario);

            return output;
        }
        private static Task ValidarCantidad(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a retirar debe ser mayor que cero.");
            }
            return Task.CompletedTask;
        }

        private static Task ValidarDisponible(int stock, int cantidadRetirar)
        {
            if (stock < cantidadRetirar)
            {
                throw new ArgumentException("La cantidad a retirar es mayor a la disponible en el stock.");
            }
            return Task.CompletedTask;
        }
    }
}

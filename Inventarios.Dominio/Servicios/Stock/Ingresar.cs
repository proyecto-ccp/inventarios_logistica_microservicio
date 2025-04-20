
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Repositorios;

namespace Inventarios.Dominio.Servicios.Stock
{
    public class Ingresar(IInventarioRepositorio inventario)
    {
        private readonly IInventarioRepositorio _inventario = inventario;

        public async Task<Inventario> Ejecutar(Inventario input)
        {
            Inventario output;

            await ValidarCantidad(input.CantidadStock);
            var productoEnInventario = await ValidarExistencia(input.IdProducto);

            if (productoEnInventario is null)
            {
                input.Id = Guid.NewGuid();
                input.FechaCreacion = DateTime.Now;
                output = await _inventario.CrearStock(input);
            }
            else 
            {
                productoEnInventario.CantidadStock += input.CantidadStock;
                productoEnInventario.FechaModificacion = DateTime.Now;
                output = await _inventario.ActualizarStock(productoEnInventario);
            }

            return output;
        }

        private static Task ValidarCantidad(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a ingresar debe ser mayor que cero.");
            }
            return Task.CompletedTask;
        }

        private async Task<Inventario> ValidarExistencia(int idProducto)
        {
            return await _inventario.ConsultarStock(idProducto);
        }   


    }
}

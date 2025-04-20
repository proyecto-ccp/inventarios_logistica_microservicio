
using Inventarios.Aplicacion.Stock.Dto;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Aplicacion.Stock.Consultas
{
    [ExcludeFromCodeCoverage]
    public record StockProductoConsulta(int IdProducto) : IRequest<InventarioOut>;

}

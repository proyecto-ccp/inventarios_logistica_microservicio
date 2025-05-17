
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Aplicacion.Comun;
using System.Text.Json.Serialization;

namespace Inventarios.Aplicacion.Stock.Comandos
{
    [ExcludeFromCodeCoverage]
    public record IngresarStock(
        
        [Required(ErrorMessage = "El campo IdProducto es obligatorio")]
        int IdProducto,
        [Required(ErrorMessage = "El campo CantidadStock es obligatorio")]
        int CantidadStock,
        [property: JsonIgnore]
        BaseIn Control

        ) : IRequest<InventarioOut>;

    [ExcludeFromCodeCoverage]
    public record DisminuirStock(

        [Required(ErrorMessage = "El campo IdProducto es obligatorio")]
        int IdProducto,
        [Required(ErrorMessage = "El campo CantidadStock es obligatorio")]
        int CantidadStock,
        [property: JsonIgnore]
        BaseIn Control

    ) : IRequest<InventarioOut>;

}

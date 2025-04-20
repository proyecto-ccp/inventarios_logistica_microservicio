using Inventarios.Aplicacion.Comun;
using System.Diagnostics.CodeAnalysis;


namespace Inventarios.Aplicacion.Stock.Dto
{
    [ExcludeFromCodeCoverage]
    public class InventarioDto
    {
        public Guid Id { get; set; }
        public int IdProducto { get; set; }
        public int CantidadStock { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }

    [ExcludeFromCodeCoverage]
    public class InventarioOut : BaseOut
    {
        public InventarioDto Inventario { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ListaInventarioOut : BaseOut
    {
        public List<InventarioDto> Inventarios { get; set; }
    }
}

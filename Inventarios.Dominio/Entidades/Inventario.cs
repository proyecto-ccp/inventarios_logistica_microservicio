

namespace Inventarios.Dominio.Entidades
{
    public class Inventario : EntidadBaseGuid
    {
        public int IdProducto { get; set; }
        public int CantidadStock { get; set; }

    }
}

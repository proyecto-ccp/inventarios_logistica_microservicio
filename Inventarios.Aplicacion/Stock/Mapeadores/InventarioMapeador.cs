using AutoMapper;
using Inventarios.Aplicacion.Stock.Comandos;
using Inventarios.Aplicacion.Stock.Consultas;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;


namespace Inventarios.Aplicacion.Stock.Mapeadores
{
    public class InventarioMapeador: Profile
    {
        public InventarioMapeador()
        {
            CreateMap<Inventario, InventarioDto>().ReverseMap();
            CreateMap<IngresarStock, Inventario>().ReverseMap();
            CreateMap<DisminuirStock, Inventario>().ReverseMap();
            CreateMap<StockProductoConsulta, Inventario>().ReverseMap();    
        }
    }
}

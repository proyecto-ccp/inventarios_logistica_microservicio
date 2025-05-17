using AutoMapper;
using Inventarios.Aplicacion.Stock.Comandos;
using Inventarios.Aplicacion.Stock.Consultas;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.ObjetoValor;


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

            CreateMap<IngresarStock, Auditoria>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Control.IdUsuario))
                .ForMember(dest => dest.Accion, opt => opt.MapFrom(src => "Productos ingresados"))
                .ForMember(dest => dest.TablaAfectada, opt => opt.MapFrom(src => "tbl_inventario"));

            CreateMap<DisminuirStock, Auditoria>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Control.IdUsuario))
                .ForMember(dest => dest.Accion, opt => opt.MapFrom(src => "Productos retirados"))
                .ForMember(dest => dest.TablaAfectada, opt => opt.MapFrom(src => "tbl_inventario"));
        }
    }
}

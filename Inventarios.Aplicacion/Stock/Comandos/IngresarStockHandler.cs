
using AutoMapper;
using MediatR;
using Inventarios.Aplicacion.Comun;
using Inventarios.Dominio.Servicios.Stock;
using System.Net;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;

namespace Inventarios.Aplicacion.Stock.Comandos
{
    public class IngresarStockHandler : IRequestHandler<IngresarStock, InventarioOut>
    {
        private readonly IMapper _mapper;
        private readonly Ingresar _servicio;

        public IngresarStockHandler(IMapper mapper, Ingresar servicio)
        {
            _mapper = mapper;
            _servicio = servicio;
        }
        public async Task<InventarioOut> Handle(IngresarStock request, CancellationToken cancellationToken)
        {
            InventarioOut output = new();

            try
            {
                var ingresoStock = _mapper.Map<Inventario>(request);
                output.Inventario = _mapper.Map<InventarioDto>(await _servicio.Ejecutar(ingresoStock));
                output.Resultado = Resultado.Exitoso;
                output.Mensaje = "El stock del producto aumento correctamente";
                output.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                output.Resultado = Resultado.Error;
                output.Mensaje = string.Concat("Message: ", ex.Message, ex.InnerException is null ? "" : "-InnerException-"+ex.InnerException.Message);
                output.Status = HttpStatusCode.InternalServerError;
            }

            return output;
        }
    }
}

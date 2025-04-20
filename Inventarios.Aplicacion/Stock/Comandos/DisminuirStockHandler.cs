
using AutoMapper;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Servicios.Stock;
using MediatR;
using System.Net;

namespace Inventarios.Aplicacion.Stock.Comandos
{
    public class DisminuirStockHandler : IRequestHandler<DisminuirStock, InventarioOut>
    {
        private readonly IMapper _mapper;
        private readonly Retirar _servicio;

        public DisminuirStockHandler(IMapper mapper, Retirar servicio)
        {
            _mapper = mapper;
            _servicio = servicio;
        }
        public async Task<InventarioOut> Handle(DisminuirStock request, CancellationToken cancellationToken)
        {
            InventarioOut output = new();

            try
            {
                var retiroStock = _mapper.Map<Inventario>(request);
                output.Inventario = _mapper.Map<InventarioDto>(await _servicio.Ejecutar(retiroStock));
                output.Resultado = Resultado.Exitoso;
                output.Mensaje = "El stock del producto disminuyo correctamente";
                output.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                output.Resultado = Resultado.Error;
                output.Mensaje = string.Concat("Message: ", ex.Message, ex.InnerException is null ? "" : "-InnerException-" + ex.InnerException.Message);
                output.Status = HttpStatusCode.InternalServerError;
            }

            return output;
        }
    }
}

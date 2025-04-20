
using AutoMapper;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Servicios.Stock;
using MediatR;
using System.Net;

namespace Inventarios.Aplicacion.Stock.Consultas
{
    public class StockProductoConsultaHandler : IRequestHandler<StockProductoConsulta, InventarioOut>
    {
        private readonly IMapper _mapper;
        private readonly Consultar _servicio;

        public StockProductoConsultaHandler(Consultar servicio, IMapper mapper) 
        {
            _mapper = mapper;
            _servicio = servicio;
        }
        public async Task<InventarioOut> Handle(StockProductoConsulta request, CancellationToken cancellationToken)
        {
            InventarioOut output = new ();

            try 
            {
                var consultaStock = await _servicio.Ejecutar(request.IdProducto);
                output.Inventario = _mapper.Map<InventarioDto>(consultaStock);
                output.Resultado = Resultado.Exitoso;
                output.Mensaje = "Consulta exitosa";
                output.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                output.Resultado = Resultado.Error;
                output.Mensaje = ex.Message;
                output.Status = HttpStatusCode.InternalServerError;
            }

            return output;

        }
    }
}

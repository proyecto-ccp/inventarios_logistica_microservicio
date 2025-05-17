
using AutoMapper;
using MediatR;
using Inventarios.Aplicacion.Comun;
using Inventarios.Dominio.Servicios.Stock;
using System.Net;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Integraciones;
using Inventarios.Dominio.ObjetoValor;
using System.Text.Json;

namespace Inventarios.Aplicacion.Stock.Comandos
{
    public class IngresarStockHandler : IRequestHandler<IngresarStock, InventarioOut>
    {
        private readonly IMapper _mapper;
        private readonly Ingresar _servicio;
        private readonly IServicioAuditoriaApi _servicioAuditoriaApi;

        public IngresarStockHandler(IMapper mapper, Ingresar servicio, IServicioAuditoriaApi servicioAuditoriaApi)
        {
            _mapper = mapper;
            _servicio = servicio;
            _servicioAuditoriaApi = servicioAuditoriaApi;
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

                var inputAuditoria = _mapper.Map<Auditoria>(request);
                inputAuditoria.IdRegistro = output.Inventario.Id.ToString();
                inputAuditoria.Registro = JsonSerializer.Serialize(output.Inventario);
                _ = Task.Run(() => _servicioAuditoriaApi.RegistrarAuditoria(inputAuditoria), cancellationToken);
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

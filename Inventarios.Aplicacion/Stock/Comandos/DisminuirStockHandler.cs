
using AutoMapper;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Dto;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.ObjetoValor;
using Inventarios.Dominio.Puertos.Integraciones;
using Inventarios.Dominio.Servicios.Stock;
using MediatR;
using System.Net;
using System.Text.Json;

namespace Inventarios.Aplicacion.Stock.Comandos
{
    public class DisminuirStockHandler : IRequestHandler<DisminuirStock, InventarioOut>
    {
        private readonly IMapper _mapper;
        private readonly Retirar _servicio;
        private readonly IServicioAuditoriaApi _servicioAuditoriaApi;

        public DisminuirStockHandler(IMapper mapper, Retirar servicio, IServicioAuditoriaApi servicioAuditoriaApi)
        {
            _mapper = mapper;
            _servicio = servicio;
            _servicioAuditoriaApi = servicioAuditoriaApi;
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

                var inputAuditoria = _mapper.Map<Auditoria>(request);
                inputAuditoria.IdRegistro = output.Inventario.Id.ToString();
                inputAuditoria.Registro = JsonSerializer.Serialize(output.Inventario);
                _ = Task.Run(() => _servicioAuditoriaApi.RegistrarAuditoria(inputAuditoria), cancellationToken);
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

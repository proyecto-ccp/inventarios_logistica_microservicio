
using AutoMapper;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Comandos;
using Inventarios.Aplicacion.Stock.Mapeadores;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Integraciones;
using Inventarios.Dominio.Puertos.Repositorios;
using Inventarios.Dominio.Servicios.Stock;
using Moq;
using System.Net;

namespace Inventarios.Tests.Aplicacion.Consultas
{
    public class DisminuirStockHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Retirar _servicio;
        private readonly Mock<IServicioAuditoriaApi> mockServicioAuditoriaApi;
        private readonly Mock<IInventarioRepositorio> mockInventarioRepositorio;
        
        public DisminuirStockHandlerTest() 
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new InventarioMapeador()));
            _mapper = config.CreateMapper();
            mockServicioAuditoriaApi = new Mock<IServicioAuditoriaApi>();
            mockInventarioRepositorio = new Mock<IInventarioRepositorio>();
            _servicio = new Retirar(mockInventarioRepositorio.Object);
        }

        [Theory]
        [InlineData(Resultado.Exitoso, "El stock del producto disminuyo correctamente", HttpStatusCode.OK, 10, 5)]
        [InlineData(Resultado.Error, "La cantidad a retirar debe ser mayor que cero", HttpStatusCode.InternalServerError, 10, 0)]
        [InlineData(Resultado.Error, "La cantidad a retirar es mayor a la disponible en el stock", HttpStatusCode.InternalServerError, 10, 20)]
        public async Task Handle_ValidaRespuestas(Resultado res, string msj, HttpStatusCode status, int existe, int retira)
        {
            Inventario output = new()
            {
                Id = Guid.NewGuid(),
                IdProducto = 1,
                CantidadStock = existe,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            if (res == Resultado.Exitoso)
            {
                mockInventarioRepositorio.SetupSequence(m => m.ConsultarStock(It.IsAny<int>()))
                    .ReturnsAsync(output)
                    .ReturnsAsync(output);
                mockInventarioRepositorio.Setup(m => m.ActualizarStock(It.IsAny<Inventario>())).ReturnsAsync(output);
            }
            else if (res == Resultado.Error)
            {
                mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ThrowsAsync(new Exception(msj));
            }

            var objPrueba = new DisminuirStockHandler(_mapper, _servicio, mockServicioAuditoriaApi.Object);
            var baseIn = new BaseIn
            {
                Token = "tokenpruebas",
                IdUsuario = Guid.NewGuid().ToString(),
            };
            var request = new DisminuirStock(1, retira, baseIn);

            var result = await objPrueba.Handle(request, CancellationToken.None);

            Assert.Equal(res, result.Resultado);
            Assert.Contains(msj, result.Mensaje);
            Assert.Equal(status, result.Status);

            if (res == Resultado.Exitoso)
            {
                Assert.NotNull(result.Inventario);
                Assert.Equal(5, result.Inventario.CantidadStock);
            }
            
        }
    }

    
}

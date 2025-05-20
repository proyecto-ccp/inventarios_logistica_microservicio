
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
    public class IngresarStockHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Ingresar _servicio;
        private readonly Mock<IServicioAuditoriaApi> mockServicioAuditoriaApi;
        private readonly Mock<IInventarioRepositorio> mockInventarioRepositorio;

        public IngresarStockHandlerTest() 
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new InventarioMapeador()));
            _mapper = config.CreateMapper();
            mockServicioAuditoriaApi = new Mock<IServicioAuditoriaApi>();
            mockInventarioRepositorio = new Mock<IInventarioRepositorio>();
            _servicio = new Ingresar(mockInventarioRepositorio.Object);

        }

        [Theory]
        [InlineData(Resultado.Exitoso, "El stock del producto aumento correctamente", HttpStatusCode.OK, 0, 10)]
        [InlineData(Resultado.Exitoso, "El stock del producto aumento correctamente", HttpStatusCode.OK, 5, 5)]
        [InlineData(Resultado.Error, "La cantidad a ingresar debe ser mayor que cero", HttpStatusCode.InternalServerError, 0, 0)]
        public async Task Handle_ValidaRespuestas(Resultado res, string msj, HttpStatusCode status, int existe, int ingresa)
        {
            Inventario output = new()
            {
                Id = Guid.NewGuid(),
                IdProducto = 1,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            if (res == Resultado.Exitoso && existe == 0)
            {
                mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ReturnsAsync((Inventario)null);
                output.CantidadStock = ingresa;
                mockInventarioRepositorio.Setup(m => m.CrearStock(It.IsAny<Inventario>())).ReturnsAsync(output);
            }
            if (res == Resultado.Exitoso && existe > 0)
            {
                output.CantidadStock = existe;
                mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ReturnsAsync(output);
                mockInventarioRepositorio.Setup(m => m.ActualizarStock(It.IsAny<Inventario>())).ReturnsAsync(output);
            }
            else if (res == Resultado.Error)
            {
                mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ThrowsAsync(new Exception(msj));
            }

            var objPrueba = new IngresarStockHandler(_mapper, _servicio, mockServicioAuditoriaApi.Object);
            var baseIn = new BaseIn
            {
                Token = "tokenpruebas",
                IdUsuario = Guid.NewGuid().ToString(),
            };
            var request = new IngresarStock(1, ingresa, baseIn);

            var result = await objPrueba.Handle(request, CancellationToken.None);

            Assert.Equal(res, result.Resultado);
            Assert.Contains(msj, result.Mensaje);
            Assert.Equal(status, result.Status);

            if (res == Resultado.Exitoso)
            {
                Assert.NotNull(result.Inventario);
                Assert.Equal(10, result.Inventario.CantidadStock);
            }

        }

    }
}


using AutoMapper;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Consultas;
using Inventarios.Aplicacion.Stock.Mapeadores;
using Inventarios.Dominio.Entidades;
using Inventarios.Dominio.Puertos.Repositorios;
using Inventarios.Dominio.Servicios.Stock;
using Moq;
using System.Net;

namespace Inventarios.Tests.Aplicacion.Consultas
{
    public class StockProductoConsultaHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Consultar _servicio;
        private readonly Mock<IInventarioRepositorio> _mockInventarioRepositorio;

        public StockProductoConsultaHandlerTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new InventarioMapeador()));
            _mapper = config.CreateMapper();
            _mockInventarioRepositorio = new Mock<IInventarioRepositorio>();
            _servicio = new Consultar(_mockInventarioRepositorio.Object);
        }

        [Theory]
        [InlineData(Resultado.Exitoso, "Consulta exitosa", HttpStatusCode.OK)]
        [InlineData(Resultado.Error, "Error", HttpStatusCode.InternalServerError)]
        public async Task Handle_ValidaRespuestas(Resultado res, string msj, HttpStatusCode status) 
        {
            Inventario output = new Inventario
            {
                IdProducto = 1,
                CantidadStock = 10,
                FechaCreacion = DateTime.Now,
            };
            if (res == Resultado.Exitoso)
            {
                _mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ReturnsAsync(output);
            }
            else if (res == Resultado.Error)
            {
                _mockInventarioRepositorio.Setup(m => m.ConsultarStock(It.IsAny<int>())).ThrowsAsync(new Exception(msj));
            }

            var objPrueba = new StockProductoConsultaHandler(_servicio, _mapper);
            var request = new StockProductoConsulta(1);

            var result = await objPrueba.Handle(request, CancellationToken.None);

            Assert.Equal(res, result.Resultado);
            Assert.Contains(msj, result.Mensaje);
            Assert.Equal(status, result.Status);

            if (res == Resultado.Exitoso)
            {
                Assert.NotNull(result.Inventario);
                Assert.Equal(output.CantidadStock, result.Inventario.CantidadStock);
            }
        }
    }
}

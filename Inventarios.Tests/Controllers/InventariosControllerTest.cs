
using Inventarios.Api.Controllers;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Comandos;
using Inventarios.Aplicacion.Stock.Consultas;
using Inventarios.Aplicacion.Stock.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Inventarios.Tests.Controllers
{
    public class InventariosControllerTest
    {
        private readonly Mock<IMediator> mockMediator;

        public InventariosControllerTest() 
        {
            mockMediator = new Mock<IMediator>();
        }

        [Theory]
        [InlineData(Resultado.Exitoso, HttpStatusCode.OK)]
        [InlineData(Resultado.Error, HttpStatusCode.InternalServerError)]
        public async Task Consultar_respuestas(Resultado enumRes, HttpStatusCode status)
        {
            var output = new InventarioOut
            {
                Resultado = enumRes,
                Status = status
            };

            mockMediator.Setup(m => m.Send(It.IsAny<StockProductoConsulta>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(output);

            var objPrueba = new InventariosController(mockMediator.Object);

            var request = new StockProductoConsulta(100);

            var resultado = await objPrueba.Consultar(request);

            Assert.NotNull(resultado);

            if (enumRes == Resultado.Exitoso)
            {
                var verResultado = Assert.IsType<OkObjectResult>(resultado);
                var res = verResultado.Value as InventarioOut;
                Assert.IsType<InventarioOut>(res);
                Assert.Equal(200, verResultado.StatusCode);
            }
            else 
            {
                var verResultado = Assert.IsType<ObjectResult>(resultado);
                var res = verResultado.Value as ProblemDetails;
                Assert.IsType<ProblemDetails>(res);
                Assert.Equal(500, verResultado.StatusCode);
            }

        }

        [Theory]
        [InlineData(Resultado.Exitoso, HttpStatusCode.Created)]
        [InlineData(Resultado.Error, HttpStatusCode.InternalServerError)]
        public async Task Ingresar_respuestas(Resultado enumRes, HttpStatusCode status)
        {
            var output = new InventarioOut
            {
                Resultado = enumRes,
                Status = status
            };

            mockMediator.Setup(m => m.Send(It.IsAny<IngresarStock>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(output);

            var objPrueba = new InventariosController(mockMediator.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = "Bearer pruebas-token-123";
            httpContext.Items["UserId"] = Guid.NewGuid().ToString();
            objPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var baseIn = new BaseIn
            {
                Token = "tokenPruebasUnitarias",
                IdUsuario = Guid.NewGuid().ToString(),
            };
            var request = new IngresarStock
            (
                1,
                100,
                baseIn
            );

            var resultado = await objPrueba.Ingresar(request);

            Assert.NotNull(resultado);

            if (enumRes == Resultado.Exitoso)
            {
                var verResultado = Assert.IsType<OkObjectResult>(resultado);
                var res = verResultado.Value as InventarioOut;
                Assert.IsType<InventarioOut>(res);
                Assert.Equal(200, verResultado.StatusCode);
            }
            else
            {
                var verResultado = Assert.IsType<ObjectResult>(resultado);
                var res = verResultado.Value as ProblemDetails;
                Assert.IsType<ProblemDetails>(res);
                Assert.Equal(500, verResultado.StatusCode);
            }
        }

        [Theory]
        [InlineData(Resultado.Exitoso, HttpStatusCode.Created)]
        [InlineData(Resultado.Error, HttpStatusCode.InternalServerError)]
        public async Task Retirar_respuestas(Resultado enumRes, HttpStatusCode status)
        {
            var output = new InventarioOut
            {
                Resultado = enumRes,
                Status = status
            };

            mockMediator.Setup(m => m.Send(It.IsAny<DisminuirStock>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(output);

            var objPrueba = new InventariosController(mockMediator.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Authorization = "Bearer pruebas-token-123";
            httpContext.Items["UserId"] = Guid.NewGuid().ToString();
            objPrueba.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var baseIn = new BaseIn
            {
                Token = "tokenPruebasUnitarias",
                IdUsuario = Guid.NewGuid().ToString(),
            };
            var request = new DisminuirStock
            (
                1,
                100,
                baseIn
            );

            var resultado = await objPrueba.Retirar(request);

            Assert.NotNull(resultado);

            if (enumRes == Resultado.Exitoso)
            {
                var verResultado = Assert.IsType<OkObjectResult>(resultado);
                var res = verResultado.Value as InventarioOut;
                Assert.IsType<InventarioOut>(res);
                Assert.Equal(200, verResultado.StatusCode);
            }
            else
            {
                var verResultado = Assert.IsType<ObjectResult>(resultado);
                var res = verResultado.Value as ProblemDetails;
                Assert.IsType<ProblemDetails>(res);
                Assert.Equal(500, verResultado.StatusCode);
            }
        }
    }
}


using Inventarios.Api.Helpers;
using Inventarios.Aplicacion.Comun;
using Inventarios.Aplicacion.Stock.Comandos;
using Inventarios.Aplicacion.Stock.Consultas;
using Inventarios.Aplicacion.Stock.Dto;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Inventarios.Api.Controllers
{
    /// <summary>
    /// Controlador de inventarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize]
    public class InventariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// constructor
        /// </summary>
        public InventariosController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consultar stock de un producto
        /// </summary>
        /// <response code="200"> 
        /// InventarioOut: objeto de salida <br/>
        /// Resultado: Enumerador de la operación, Exitoso = 1, Error = 2, SinRegistros = 3 <br/>
        /// Mensaje: Mensaje de la operación <br/>
        /// Status: Código de estado HTTP <br/>
        /// </response>
        [HttpGet]
        [Route("Consultar")]
        [ProducesResponseType(typeof(InventarioOut), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> Consultar([FromQuery] StockProductoConsulta input)
        {
            var output = await _mediator.Send(input);

            if (output.Resultado != Resultado.Error)
            {
                return Ok(output);
            }
            else
            {
                return Problem(output.Mensaje, statusCode: (int)output.Status);
            }
        }

        /// <summary>
        /// Ingresar productos al inventario
        /// </summary>
        /// <response code="200"> 
        /// InventarioOut: objeto de salida <br/>
        /// Resultado: Enumerador de la operación, Exitoso = 1, Error = 2, SinRegistros = 3 <br/>
        /// Mensaje: Mensaje de la operación <br/>
        /// Status: Código de estado HTTP <br/>
        /// </response>
        [HttpPost]
        [Route("Ingresar")]
        [ProducesResponseType(typeof(InventarioOut), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> Ingresar([FromBody] IngresarStock input)
        {
            var baseIn = new BaseIn
            {
                Token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", ""),
                IdUsuario = HttpContext.Items["UserId"].ToString()
            };
            var inputBaseIn = input with { Control = baseIn };

            var output = await _mediator.Send(inputBaseIn);

            if (output.Resultado != Resultado.Error)
            {
                return Ok(output);
            }
            else
            {
                return Problem(output.Mensaje, statusCode: (int)output.Status);
            }
        }

        /// <summary>
        /// Retirar productos del inventario
        /// </summary>
        /// <response code="200"> 
        /// InventarioOut: objeto de salida <br/>
        /// Resultado: Enumerador de la operación, Exitoso = 1, Error = 2, SinRegistros = 3 <br/>
        /// Mensaje: Mensaje de la operación <br/>
        /// Status: Código de estado HTTP <br/>
        /// </response>
        [HttpPost]
        [Route("Retirar")]
        [ProducesResponseType(typeof(InventarioOut), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 401)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> Retirar([FromBody] DisminuirStock input)
        {
            var baseIn = new BaseIn
            {
                Token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", ""),
                IdUsuario = HttpContext.Items["UserId"].ToString()
            };
            var inputBaseIn = input with { Control = baseIn };

            var output = await _mediator.Send(inputBaseIn);

            if (output.Resultado != Resultado.Error)
            {
                return Ok(output);
            }
            else
            {
                return Problem(output.Mensaje, statusCode: (int)output.Status);
            }
        }


    }
}

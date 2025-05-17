using Inventarios.Dominio.Puertos.Integraciones;
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Api.Middleware
{
    /// <summary>
    /// Middleware para validar el token de autorización
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AutorizadorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServicioUsuariosApi _autorizador;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="autorizador"></param>
        public AutorizadorMiddleware(RequestDelegate next, IServicioUsuariosApi autorizador)
        {
            _next = next;
            _autorizador = autorizador;
        }
        /// <summary>
        /// Procesa el request y valida el token
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(token)) 
            {
                context.Items["UserId"] = await Validar(token);
            }

            await _next(context);
        }

        private async Task<string> Validar(string token)
        {
            string usuarioToken = null;
            try
            {
                var resultado = await _autorizador.ValidarToken(token);

                if (resultado.Status == System.Net.HttpStatusCode.OK) 
                { 
                    usuarioToken = resultado.IdUsuario;   
                }
            }
            catch 
            {
                usuarioToken = null;
            }

            return usuarioToken;
        }
    }
}

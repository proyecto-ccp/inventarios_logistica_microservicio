using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Api.Helpers
{
    /// <summary>
    /// Middleware para validar el token de autorización
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// verifica si el usuario está autenticado
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Items["UserId"];
            var header = context.HttpContext.Request.Headers.Authorization;

            if (string.IsNullOrEmpty(header))
            {
                context.Result = new JsonResult(new { message = "El Cabecero Authorization es requerido" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else if (userId == null)
            {
                context.Result = new JsonResult(new { message = "Acceso no autorizado" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
    
}

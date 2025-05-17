
using Microsoft.Extensions.Configuration;
using Inventarios.Dominio.ObjetoValor;
using Inventarios.Dominio.Puertos.Integraciones;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Inventarios.Infraestructura.Adaptadores.Integraciones
{
    [ExcludeFromCodeCoverage]
    public class ServicioUsuariosApi : IServicioUsuariosApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ServicioUsuariosApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<TokenInfo> ValidarToken(string token)
        {
            var uri = _configuration["UriAutorizador"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respuesta = await _httpClient.GetAsync($"{uri}");
            respuesta.EnsureSuccessStatusCode();
            var objRespuesta = await respuesta.Content.ReadFromJsonAsync<TokenInfo>();
            return objRespuesta;

        }
    }
}

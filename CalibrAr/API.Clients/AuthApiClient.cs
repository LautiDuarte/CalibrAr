using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class AuthApiClient : BaseApiClient
    {
        // Anónimo: el login todavía no tiene token.
        public AuthApiClient() : base(null)
        {
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            using var httpClient = await CreateHttpClientAsync();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            // 401 = credenciales incorrectas: es una respuesta esperada, no un error.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return null;

            // Cualquier otro código es una falla del servidor y no debe confundirse con una contraseña mal escrita.
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed. Status code: {response.StatusCode}, Error: {errorContent}");
        }
    }
}

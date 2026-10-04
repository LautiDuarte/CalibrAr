using System.Net;
using System.Net.Http.Headers;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        // null = cliente anónimo (por ejemplo el login, que todavía no tiene token).
        private readonly ITokenProvider? tokenProvider;

        protected BaseApiClient(ITokenProvider? tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        protected async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient();
            await ConfigureHttpClientAsync(client);
            return client;
        }

        private async Task ConfigureHttpClientAsync(HttpClient client)
        {
            // Leer URL base de configuración, si no existe usar localhost por defecto
            string baseUrl = GetBaseUrlFromConfig();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // El token se lo pedimos a quien usa el cliente (WinForms o Blazor),
            // nunca a un estático global compartido.
            if (tokenProvider != null)
            {
                var token = await tokenProvider.GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        private static string GetBaseUrlFromConfig()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Attempting on reading configuration...");

                // 1. Primero revisar variable de entorno
                string? envUrl = Environment.GetEnvironmentVariable("TPI_API_BASE_URL");
                if (!string.IsNullOrEmpty(envUrl))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] URL from environment variable: {envUrl}");
                    return envUrl;
                }

                // 2. Detectar si estamos en Android por el runtime
                string runtimeInfo = System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Runtime: {runtimeInfo}");

                if (runtimeInfo.StartsWith("android"))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Android detected - using emulator IP");
                    return "http://10.0.2.2:5183/";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Error detecting platform: {ex.Message}");
            }

            // URL por defecto para Windows/otras plataformas
            string defaultUrl = "http://localhost:5031/";
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Using default URL: {defaultUrl}");
            return defaultUrl;
        }

        // Ya no cierra la sesión: solo avisa con la excepción y cada UI decide qué hacer
        // (WinForms vuelve al login, Blazor redirige a /login).
        protected static void ThrowIfUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Your session has expired.");
            }
        }
    }
}

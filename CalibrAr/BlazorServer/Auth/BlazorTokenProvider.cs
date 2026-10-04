using API.Clients;
using BlazorServer.Components.Pages;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorServer.Auth
{
    // Provee a los ApiClient el JWT del usuario actual, guardado como claim en la cookie.
    // Se lee del AuthenticationStateProvider (y no de IHttpContextAccessor) porque HttpContext
    // solo existe en el request inicial: dentro del circuito no hay request HTTP.
    // Scoped: una instancia por circuito, o sea por usuario.
    public class BlazorTokenProvider : ITokenProvider
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public BlazorTokenProvider(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string?> GetTokenAsync()
        {
            // Si la revalidación detectó el vencimiento, el usuario ya es anónimo y devuelve null.
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User.FindFirst(Login.AccessTokenClaim)?.Value;
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using BlazorServer.Components.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace BlazorServer.Auth
{
    // En un circuito interactivo el usuario se lee una sola vez (de la cookie, en el request
    // inicial). Este proveedor lo revisa periódicamente y, si el JWT venció, pasa la UI a
    // usuario anónimo: AuthorizeRouteView redirige al login sin esperar un 401 de la WebAPI.
    public class TokenExpirationAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        public TokenExpirationAuthenticationStateProvider(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        // Cada cuánto se revisa. 1 minuto = como mucho 1 minuto de UI "desactualizada".
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(1);

        protected override Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var token = authenticationState.User.FindFirst(Login.AccessTokenClaim)?.Value;
            if (string.IsNullOrEmpty(token))
                return Task.FromResult(false);

            // ValidTo = el "exp" del JWT. Solo comparamos la fecha: la firma la valida la WebAPI.
            var expiresAt = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
            return Task.FromResult(DateTime.UtcNow < expiresAt);
        }
    }
}

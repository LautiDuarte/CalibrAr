using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorServer.Auth
{
    // Id del usuario de la sesión, leído de la cookie. Lo piden algunas operaciones de la API
    // (por ejemplo, quién cambió el estado de un instrumento). Scoped: uno por usuario.
    public class CurrentUser
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public CurrentUser(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<int?> GetIdAsync()
        {
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            var value = state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(value, out var id) ? id : null;
        }

        // El claim "group" trae el nombre del grupo de permisos tal cual está en la base.
        public async Task<bool> IsAdministratorAsync()
        {
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User.FindFirst("group")?.Value == "Administrator";
        }
    }
}

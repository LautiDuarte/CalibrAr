using System.Security.Claims;

namespace WebAPI
{
    public static class ClaimsPrincipalExtensions
    {
        // El id del usuario sale siempre del token (claim NameIdentifier), nunca de la URL ni del cuerpo.
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Clients
{
    public interface IAuthService
    {
        event Action<bool>? AuthenticationStateChanged;

        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<string?> GetEmailAsync();
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task CheckTokenExpirationAsync();
        Task<bool> HasPermissionAsync(string permission);
    }
}

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

        bool IsAuthenticated();
        string? GetToken();
        string? GetEmail();
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task CheckTokenExpirationAsync();
        bool HasPermission(string permission);
    }
}

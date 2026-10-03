using DTOs;
using API.Clients;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Auth.WindowsForms
{
    public class WindowsFormsAuthService : IAuthService
    {
        private static string? _currentToken;
        private static DateTime _tokenExpiration;
        private static string? _currentEmail;

        public event Action<bool>? AuthenticationStateChanged;

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow < _tokenExpiration;
        }

        public string? GetToken()
        {
            var isAuth = IsAuthenticated();
            return isAuth ? _currentToken : null;
        }

        public string? GetEmail()
        {
            var isAuth = IsAuthenticated();
            return isAuth ? _currentEmail : null;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var authClient = new AuthApiClient();
            var response = await authClient.LoginAsync(request);

            if (response != null)
            {
                _currentToken = response.Token;
                _tokenExpiration = response.ExpiresAt;
                _currentEmail = response.Email;

                AuthenticationStateChanged?.Invoke(true);
                return true;
            }

            return false;
        }

        public Task LogoutAsync()
        {
            _currentToken = null;
            _tokenExpiration = default;
            _currentEmail = null;

            AuthenticationStateChanged?.Invoke(false);
            return Task.CompletedTask;
        }

        public async Task CheckTokenExpirationAsync()
        {
            if (!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow >= _tokenExpiration)
            {
                await LogoutAsync();
            }
        }

        public bool HasPermission(string permission)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);

                // Buscar claims de "permission"
                var permissionClaims = jsonToken.Claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value);

                return permissionClaims.Contains(permission);
            }
            catch
            {
                return false;
            }
        }
    }
}
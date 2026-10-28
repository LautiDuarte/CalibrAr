using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Clients;

namespace API.Auth.WindowsForms
{
    // Estático global: válido en escritorio (un proceso = un usuario), nunca en Blazor Server.
    public static class AuthServiceProvider
    {
        private static IAuthService? _instance;
        private static readonly object _lock = new object();

        public static IAuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException(
                        "AuthService has not been registered. Call AuthServiceProvider.Register() first.");
                }
                return _instance;
            }
        }

        public static void Register(IAuthService authService)
        {
            lock (_lock)
            {
                _instance = authService;
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}

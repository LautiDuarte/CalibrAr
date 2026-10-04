namespace API.Clients
{
    // Lo único que los clientes necesitan de la UI: el token del usuario actual.
    // Cada interfaz lo resuelve a su manera (WinForms desde memoria, Blazor desde la cookie).
    public interface ITokenProvider
    {
        Task<string?> GetTokenAsync();
    }
}

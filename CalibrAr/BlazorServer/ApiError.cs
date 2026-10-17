using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlazorServer
{
    public static class ApiError
    {
        // Los ApiClient arman mensajes del tipo "... Error: {"error":"..."}".
        // Devuelve solo el texto del error si viene en ese formato.
        public static string Message(Exception ex)
        {
            int start = ex.Message.IndexOf('{');
            if (start >= 0)
            {
                try
                {
                    using var json = JsonDocument.Parse(ex.Message[start..]);
                    if (json.RootElement.TryGetProperty("error", out var error) && error.GetString() is string text)
                        return RemoveParameterSuffix(text);
                }
                catch (JsonException)
                {
                }
            }

            if (ex.Message.Contains("Status code: NotFound"))
                return "El registro solicitado no existe.";

            return ex.Message;
        }

        // Los ArgumentException del dominio terminan con " (Parameter 'email')": útil para quien programa,
        // pero no para quien usa la pantalla.
        private static string RemoveParameterSuffix(string text) =>
            Regex.Replace(text, @"\s*\(Parameter '[^']*'\)\s*$", string.Empty);
    }
}

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

            if (ex.Message.Contains("Código de estado: NotFound"))
                return "El registro solicitado no existe.";

            // Un 500 trae el detalle técnico del servidor: no se le muestra a quien usa la pantalla.
            if (IsServerError(ex))
                return "Ocurrió un error inesperado en el servidor. Intentá de nuevo.";

            return ex.Message;
        }

        public static bool IsServerError(Exception ex) => ex.Message.Contains("Código de estado: InternalServerError");

        // Los ArgumentException del dominio terminan con " (Parameter 'email')": útil para quien programa,
        // pero no para quien usa la pantalla.
        private static string RemoveParameterSuffix(string text) =>
            Regex.Replace(text, @"\s*\(Parameter '[^']*'\)\s*$", string.Empty);
    }
}

namespace BlazorServer
{
    // Mensaje de "guardado" que pasa de una pantalla a la siguiente. Se registra Scoped:
    // hay una instancia por usuario (circuito), así que un mensaje no llega a otra persona.
    public class FlashService
    {
        private string? message;

        public event Action? Changed;

        public void Success(string text)
        {
            message = text;
            Changed?.Invoke();
        }

        // Devuelve el mensaje pendiente y lo borra, para que se muestre una sola vez.
        public string? Take()
        {
            var text = message;
            message = null;
            return text;
        }
    }
}

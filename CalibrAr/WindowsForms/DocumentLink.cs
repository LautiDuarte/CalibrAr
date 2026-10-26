using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsForms
{
    // Abre en el navegador el link al PDF de un procedimiento. Solo se abren links http o https.
    public static class DocumentLink
    {
        public static void Open(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Este procedimiento no tiene un documento cargado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                MessageBox.Show("El link del documento no es válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el documento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

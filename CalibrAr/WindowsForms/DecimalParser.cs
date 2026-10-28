using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WindowsForms
{
    /// <summary>
    /// Convierte el texto que escribe el usuario a decimal sin depender de la configuración regional de Windows.
    /// Acepta "25,3" y "25.3" y en los dos casos da 25,3. No admite separador de miles.
    /// </summary>
    internal static class DecimalParser
    {
        public static bool TryParse(string? text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            string normalized = text.Trim().Replace(',', '.');
            return decimal.TryParse(
                normalized,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out value);
        }
    }
}
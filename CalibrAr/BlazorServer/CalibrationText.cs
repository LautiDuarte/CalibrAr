namespace BlazorServer
{
    // Textos y colores de los datos de una calibración, compartidos por las pantallas que los muestran.
    public static class CalibrationText
    {
        public static string InterventionText(string intervention) => intervention switch
        {
            "Calibration" => "Calibración",
            "Verification" => "Verificación",
            "Maintenance" => "Mantenimiento",
            "AdjustmentAndCalibration" => "Ajuste y calibración",
            _ => intervention
        };

        public static string ResultText(string result) => result switch
        {
            "Conformant" => "Conforme",
            "NonConformant" => "No conforme",
            "WithRestrictions" => "Con restricciones",
            _ => result
        };

        public static string ResultKind(string result) => result switch
        {
            "Conformant" => "success",
            "NonConformant" => "danger",
            "WithRestrictions" => "warning",
            _ => "secondary"
        };
    }
}

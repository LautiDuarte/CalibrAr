using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsForms
{
    // Textos en castellano de los valores internos (estados, resultados, orígenes...).
    // Solo cambian lo que se ve: lo que se guarda y lo que viaja a la API sigue siendo el valor original.
    public static class EnumText
    {
        public static string Translate(string? value) => value switch
        {
            // Estado del instrumento (CalibrationExpired también es un origen de no conformidad)
            "Active" => "Activo",
            "TemporarilyOutOfService" => "Fuera de servicio temporal",
            "CalibrationExpired" => "Calibración vencida",
            "UnderRepair" => "En reparación",
            "Decommissioned" => "Dado de baja",
            "OnLoan" => "En préstamo",

            // Tipo de intervención
            "Calibration" => "Calibración",
            "Verification" => "Verificación",
            "Maintenance" => "Mantenimiento",
            "AdjustmentAndCalibration" => "Ajuste y calibración",

            // Resultado de la calibración
            "Conformant" => "Conforme",
            "WithRestrictions" => "Conforme con restricciones",
            "NonConformant" => "No conforme",

            // Origen y estado de la no conformidad
            "NonConformantResult" => "Resultado no conforme",
            "PhysicalDamage" => "Daño físico",
            "IncorrectUse" => "Uso incorrecto",
            "Other" => "Otro",
            "Open" => "Abierta",
            "UnderAnalysis" => "En análisis",
            "WithCorrectiveActions" => "Con acciones correctivas",
            "Closed" => "Cerrada",

            _ => value ?? string.Empty
        };

        // Combo cargado con un enum: muestra el texto en castellano, pero SelectedItem sigue siendo el valor del enum.
        public static void ApplyTo(ComboBox comboBox)
        {
            comboBox.FormattingEnabled = true;
            comboBox.Format += (sender, e) => e.Value = Translate(e.ListItem?.ToString());
        }

        // Columnas de una grilla cuyo valor es el texto interno del enum.
        public static void ApplyTo(DataGridView grid, params string[] columnNames)
        {
            grid.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex >= 0 && e.Value is string text && columnNames.Contains(grid.Columns[e.ColumnIndex].Name))
                {
                    e.Value = Translate(text);
                    e.FormattingApplied = true;
                }
            };
        }
    }
}

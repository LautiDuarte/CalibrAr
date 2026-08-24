using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CalibrationMeasurementDTO
    {
        public int Id { get; set; }
        public decimal NominalValue { get; set; }
        public decimal MeasuredValue { get; set; }
        public decimal Error { get; set; }
        public bool IsWithinTolerance { get; set; }
        public string? Notes { get; set; }
        public int CalibrationId { get; set; }
    }
}
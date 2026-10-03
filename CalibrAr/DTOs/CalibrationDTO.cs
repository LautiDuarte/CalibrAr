using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CalibrationDTO
    {
        public int Id { get; set; }
        public DateTime CalibrationDate { get; set; }
        public string InterventionType { get; set; } = string.Empty;
        public bool IsExternal { get; set; }
        public string? ExternalLab { get; set; }
        public string? CertificateNumber { get; set; }
        public string Result { get; set; } = string.Empty;
        public string? RestrictionDetail { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int InstrumentId { get; set; }
        public string? InstrumentCode { get; set; }
        public int? ProcedureId { get; set; }
        public string? ProcedureName { get; set; }
        public int? PerformedByUserId { get; set; }
        public string? PerformedByUserName { get; set; }
        public int? ApprovedByUserId { get; set; }
        public string? ApprovedByUserName { get; set; }
        public List<CalibrationMeasurementDTO> Measurements { get; set; } = new List<CalibrationMeasurementDTO>();
    }
}

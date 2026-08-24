using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class NonConformityDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Status { get; set; }
        public string? CorrectiveAction { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int InstrumentId { get; set; }
        public string? InstrumentCode { get; set; }
        public int CalibrationId { get; set; }
        public string? CalibrationCertificateNumber { get; set; }
        public int DetectedByUserId { get; set; }
        public string? DetectedByUserName { get; set; }
        public int ClosedByUserId { get; set; }
        public string? ClosedByUserName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class InstrumentDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? SerialNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? MaxAllowedError { get; set; }
        public int? CalibrationFrequencyMonths { get; set; }
        public DateTime? LastCalibrationDate { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int InstrumentTypeId { get; set; }
        public string? InstrumentTypeName { get; set; }
        public int AreaId { get; set; }
        public string? AreaName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ProcedureDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string VersionNumber { get; set; }
        public DateTime ApprovedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int InstrumentTypeId { get; set; }
        public string? InstrumentTypeName { get; set; }
    }
}
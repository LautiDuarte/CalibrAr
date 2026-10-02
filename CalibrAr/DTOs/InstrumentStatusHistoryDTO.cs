using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class InstrumentStatusHistoryDTO
    {
        public int Id { get; set; }
        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public DateTime ChangedAt { get; set; }
        public int InstrumentId { get; set; }
        public string? InstrumentCode { get; set; }
        public int ChangedByUserId { get; set; }
        public string? ChangedByUserName { get; set; }
    }
}

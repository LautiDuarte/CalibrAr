using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class AreaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Responsible { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; } 
        public string? LocationAddress { get; set; } 
    }
}

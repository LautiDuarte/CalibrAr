using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ReferenceStandardDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CertifyingBody { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime CertificateIssuedAt { get; set; }
        public DateTime CertificateExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
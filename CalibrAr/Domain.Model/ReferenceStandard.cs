using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ReferenceStandard
    {
        public int Id { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public string CertifyingBody { get; private set; } = string.Empty;
        public string CertificateNumber { get; private set; } = string.Empty;
        public DateTime CertificateIssuedAt { get; private set; }
        public DateTime CertificateExpiresAt { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public virtual ICollection<Calibration> Calibrations { get; private set; } = new List<Calibration>();
        public ReferenceStandard(int id, string description, string certifyingBody, string certificateNumber,
            DateTime certificateIssuedAt, DateTime certificateExpiresAt, bool isActive, DateTime createdAt)
        {
            SetId(id);
            SetDescription(description);
            SetCertifyingBody(certifyingBody);
            SetCertificateNumber(certificateNumber);
            SetCertificateIssuedAt(certificateIssuedAt);
            SetCertificateExpiresAt(certificateExpiresAt);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
        }
        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El id debe ser mayor que 0.");
            Id = id;
        }
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía.");
            Description = description;
        }
        public void SetCertifyingBody(string certifyingBody)
        {
            if (string.IsNullOrWhiteSpace(certifyingBody))
                throw new ArgumentException("El organismo certificante no puede estar vacío.");
            CertifyingBody = certifyingBody;
        }
        public void SetCertificateNumber(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
                throw new ArgumentException("El número de certificado no puede estar vacío.");
            CertificateNumber = certificateNumber;
        }
        public void SetCertificateIssuedAt(DateTime certificateIssuedAt)
        {
            if (certificateIssuedAt == default)
                throw new ArgumentException("La fecha de emisión del certificado no puede estar vacía.");
            CertificateIssuedAt = certificateIssuedAt;
        }
        public void SetCertificateExpiresAt(DateTime certificateExpiresAt)
        {
            if (certificateExpiresAt == default)
                throw new ArgumentException("La fecha de vencimiento del certificado no puede estar vacía.");
            if (certificateExpiresAt <= CertificateIssuedAt)
                throw new ArgumentException("La fecha de vencimiento del certificado debe ser posterior a la de emisión.");
            CertificateExpiresAt = certificateExpiresAt;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de creación no puede estar vacía.");
            CreatedAt = createdAt;
        }

    }
}

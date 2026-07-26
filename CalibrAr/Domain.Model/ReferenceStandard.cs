using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Model
{
    public class ReferenceStandard
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string CertifyingBody { get; private set; }
        public string CertificateNumber { get; private set; }
        public DateTime CertificateIssuedAt { get; private set; }
        public DateTime CertificateExpiresAt { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

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
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede ser nula o vacía.", nameof(description));
            Description = description;
        }
        public void SetCertifyingBody(string certifyingBody)
        {
            if (string.IsNullOrWhiteSpace(certifyingBody))
                throw new ArgumentException("El organismo certificador no puede ser nulo o vacío.", nameof(certifyingBody));
            CertifyingBody = certifyingBody;
        }
        public void SetCertificateNumber(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
                throw new ArgumentException("El número de certificado no puede ser nulo o vacío.", nameof(certificateNumber));
            CertificateNumber = certificateNumber;
        }
        public void SetCertificateIssuedAt(DateTime certificateIssuedAt)
        {
            if (certificateIssuedAt == default)
                throw new ArgumentException("La fecha de emisión no puede ser nula.", nameof(certificateIssuedAt));
            CertificateIssuedAt = certificateIssuedAt;
        }
        public void SetCertificateExpiresAt(DateTime certificateExpiresAt)
        {
            if (certificateExpiresAt == default)
                throw new ArgumentException("La fecha de vencimiento no puede ser nula.", nameof(certificateExpiresAt));
            if (certificateExpiresAt <= CertificateIssuedAt)
                throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha de emisión.", nameof(certificateExpiresAt));
            CertificateExpiresAt = certificateExpiresAt;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de alta no puede ser nula.", nameof(createdAt));
            CreatedAt = createdAt;
        }

    }
}

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
        public string Description { get; private set; }
        public string CertifyingBody { get; private set; }
        public string CertificateNumber { get; private set; }
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
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            Id = id;
        }
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("The description cannot be null or empty.", nameof(description));
            Description = description;
        }
        public void SetCertifyingBody(string certifyingBody)
        {
            if (string.IsNullOrWhiteSpace(certifyingBody))
                throw new ArgumentException("The certifying body cannot be null or empty.", nameof(certifyingBody));
            CertifyingBody = certifyingBody;
        }
        public void SetCertificateNumber(string certificateNumber)
        {
            if (string.IsNullOrWhiteSpace(certificateNumber))
                throw new ArgumentException("The certificate number cannot be null or empty.", nameof(certificateNumber));
            CertificateNumber = certificateNumber;
        }
        public void SetCertificateIssuedAt(DateTime certificateIssuedAt)
        {
            if (certificateIssuedAt == default)
                throw new ArgumentException("The certificate issue date cannot be null.", nameof(certificateIssuedAt));
            CertificateIssuedAt = certificateIssuedAt;
        }
        public void SetCertificateExpiresAt(DateTime certificateExpiresAt)
        {
            if (certificateExpiresAt == default)
                throw new ArgumentException("The certificate expiration date cannot be null.", nameof(certificateExpiresAt));
            if (certificateExpiresAt <= CertificateIssuedAt)
                throw new ArgumentException("The certificate expiration date must be greater than the certificate issue date.", nameof(certificateExpiresAt));
            CertificateExpiresAt = certificateExpiresAt;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("The creation date cannot be null.", nameof(createdAt));
            CreatedAt = createdAt;
        }

    }
}

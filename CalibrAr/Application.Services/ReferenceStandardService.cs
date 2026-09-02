using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DTOs;
using Domain.Model;

namespace Application.Services
{
    public class ReferenceStandardService : IReferenceStandardService
    {
        private readonly IReferenceStandardRepository referenceStandardRepository;

        public ReferenceStandardService(IReferenceStandardRepository referenceStandardRepository)
        {
            this.referenceStandardRepository = referenceStandardRepository;
        }

        public async Task<ReferenceStandardDTO> AddAsync(ReferenceStandardDTO dto)
        {
            var createdAt = DateTime.Now;
            var isActive = true;
            ReferenceStandard referenceStandard = new ReferenceStandard(0, dto.Description, dto.CertifyingBody, dto.CertificateNumber, dto.CertificateIssuedAt, dto.CertificateExpiresAt, isActive, createdAt);

            await referenceStandardRepository.AddAsync(referenceStandard);

            dto.Id = referenceStandard.Id;
            dto.CreatedAt = referenceStandard.CreatedAt;
            dto.IsActive = isActive;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await referenceStandardRepository.DeleteAsync(id);
        }

        public async Task<ReferenceStandardDTO?> GetAsync(int id)
        {
            ReferenceStandard? referenceStandard = await referenceStandardRepository.GetAsync(id);

            if (referenceStandard == null)
                return null;

            return new ReferenceStandardDTO
            {
                Id = referenceStandard.Id,
                Description = referenceStandard.Description,
                CertifyingBody = referenceStandard.CertifyingBody,
                CertificateNumber = referenceStandard.CertificateNumber,
                CertificateIssuedAt = referenceStandard.CertificateIssuedAt,
                CertificateExpiresAt = referenceStandard.CertificateExpiresAt,
                IsActive = referenceStandard.IsActive,
                CreatedAt = referenceStandard.CreatedAt
            };
        }

        public async Task<IEnumerable<ReferenceStandardDTO>> GetAllAsync()
        {
            var referenceStandards = await referenceStandardRepository.GetAllAsync();
            return referenceStandards.Select(referenceStandard => new ReferenceStandardDTO
            {
                Id = referenceStandard.Id,
                Description = referenceStandard.Description,
                CertifyingBody = referenceStandard.CertifyingBody,
                CertificateNumber = referenceStandard.CertificateNumber,
                CertificateIssuedAt = referenceStandard.CertificateIssuedAt,
                CertificateExpiresAt = referenceStandard.CertificateExpiresAt,
                IsActive = referenceStandard.IsActive,
                CreatedAt = referenceStandard.CreatedAt
            });
        }

        public async Task<bool> UpdateAsync(ReferenceStandardDTO dto)
        {
            var existing = await referenceStandardRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            ReferenceStandard referenceStandard = new ReferenceStandard(dto.Id, dto.Description, dto.CertifyingBody, dto.CertificateNumber, dto.CertificateIssuedAt, dto.CertificateExpiresAt, dto.IsActive, existing.CreatedAt);
            return await referenceStandardRepository.UpdateAsync(referenceStandard);
        }
    }
}
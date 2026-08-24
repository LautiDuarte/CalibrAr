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
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository procedureRepository;
        private readonly IInstrumentTypeRepository instrumentTypeRepository;

        public ProcedureService(IProcedureRepository procedureRepository, IInstrumentTypeRepository instrumentTypeRepository)
        {
            this.procedureRepository = procedureRepository;
            this.instrumentTypeRepository = instrumentTypeRepository;
        }

        public async Task<ProcedureDTO> AddAsync(ProcedureDTO dto)
        {
            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);

            var createdAt = DateTime.Now;
            Procedure procedure = new Procedure(0, dto.Code, dto.Name, dto.VersionNumber, dto.ApprovedAt, dto.IsActive, createdAt, dto.InstrumentTypeId);

            await procedureRepository.AddAsync(procedure);

            dto.Id = procedure.Id;
            dto.CreatedAt = procedure.CreatedAt;
            dto.InstrumentTypeName = procedure.InstrumentType?.Name;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await procedureRepository.DeleteAsync(id);
        }

        public async Task<ProcedureDTO?> GetAsync(int id)
        {
            Procedure? procedure = await procedureRepository.GetAsync(id);

            if (procedure == null)
                return null;

            return new ProcedureDTO
            {
                Id = procedure.Id,
                Code = procedure.Code,
                Name = procedure.Name,
                VersionNumber = procedure.VersionNumber,
                ApprovedAt = procedure.ApprovedAt,
                IsActive = procedure.IsActive,
                CreatedAt = procedure.CreatedAt,
                InstrumentTypeId = procedure.InstrumentTypeId,
                InstrumentTypeName = procedure.InstrumentType?.Name
            };
        }

        public async Task<IEnumerable<ProcedureDTO>> GetAllAsync()
        {
            var procedures = await procedureRepository.GetAllAsync();
            return procedures.Select(procedure => new ProcedureDTO
            {
                Id = procedure.Id,
                Code = procedure.Code,
                Name = procedure.Name,
                VersionNumber = procedure.VersionNumber,
                ApprovedAt = procedure.ApprovedAt,
                IsActive = procedure.IsActive,
                CreatedAt = procedure.CreatedAt,
                InstrumentTypeId = procedure.InstrumentTypeId,
                InstrumentTypeName = procedure.InstrumentType?.Name
            });
        }

        public async Task<bool> UpdateAsync(ProcedureDTO dto)
        {
            var existing = await procedureRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            await EnsureInstrumentTypeExistsAsync(dto.InstrumentTypeId);

            Procedure procedure = new Procedure(dto.Id, dto.Code, dto.Name, dto.VersionNumber, dto.ApprovedAt, dto.IsActive, existing.CreatedAt, dto.InstrumentTypeId);
            return await procedureRepository.UpdateAsync(procedure);
        }

        private async Task EnsureInstrumentTypeExistsAsync(int instrumentTypeId)
        {
            var instrumentType = await instrumentTypeRepository.GetAsync(instrumentTypeId);
            if (instrumentType == null)
                throw new KeyNotFoundException($"No existe un InstrumentType con Id {instrumentTypeId}.");
        }
    }
}
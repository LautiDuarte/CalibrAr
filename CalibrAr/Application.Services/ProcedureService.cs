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
            await EnsureCodeAndVersionAreUniqueAsync(dto.Code, dto.VersionNumber, 0);

            var createdAt = DateTime.Now;
            var isActive = true;
            Procedure procedure = new Procedure(0, dto.Code, dto.Name, dto.VersionNumber, dto.ApprovedAt, isActive, createdAt, dto.InstrumentTypeId);

            await procedureRepository.AddAsync(procedure);

            dto.Id = procedure.Id;
            dto.CreatedAt = procedure.CreatedAt;
            dto.IsActive = isActive;
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
            await EnsureCodeAndVersionAreUniqueAsync(dto.Code, dto.VersionNumber, dto.Id);

            Procedure procedure = new Procedure(dto.Id, dto.Code, dto.Name, dto.VersionNumber, dto.ApprovedAt, dto.IsActive, existing.CreatedAt, dto.InstrumentTypeId);
            return await procedureRepository.UpdateAsync(procedure);
        }

        // Un mismo código puede repetirse en distintas versiones (una versión nueva es otro procedimiento con el
        // mismo código), pero no la combinación código + versión.
        private async Task EnsureCodeAndVersionAreUniqueAsync(string? code, string? versionNumber, int currentId)
        {
            var normalizedCode = (code ?? string.Empty).Trim();
            var normalizedVersion = (versionNumber ?? string.Empty).Trim();

            var procedures = await procedureRepository.GetAllAsync();
            if (procedures.Any(p => p.Id != currentId
                                    && string.Equals(p.Code.Trim(), normalizedCode, StringComparison.OrdinalIgnoreCase)
                                    && string.Equals(p.VersionNumber.Trim(), normalizedVersion, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Ya existe un procedimiento con el código '{normalizedCode}' en la versión '{normalizedVersion}'.");
        }

        private async Task EnsureInstrumentTypeExistsAsync(int instrumentTypeId)
        {
            var instrumentType = await instrumentTypeRepository.GetAsync(instrumentTypeId);
            if (instrumentType == null)
                throw new KeyNotFoundException($"There is no instrument type with id {instrumentTypeId}.");
        }
    }
}
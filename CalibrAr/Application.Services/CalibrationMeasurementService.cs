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
    public class CalibrationMeasurementService : ICalibrationMeasurementService
    {
        private readonly ICalibrationMeasurementRepository calibrationMeasurementRepository;
        private readonly ICalibrationRepository calibrationRepository;

        public CalibrationMeasurementService(ICalibrationMeasurementRepository calibrationMeasurementRepository, ICalibrationRepository calibrationRepository)
        {
            this.calibrationMeasurementRepository = calibrationMeasurementRepository;
            this.calibrationRepository = calibrationRepository;
        }

        public async Task<CalibrationMeasurementDTO> AddAsync(CalibrationMeasurementDTO dto)
        {
            await EnsureCalibrationExistsAsync(dto.CalibrationId);

            CalibrationMeasurement calibrationMeasurement = new CalibrationMeasurement(0, dto.NominalValue, dto.MeasuredValue, dto.Error, dto.IsWithinTolerance, dto.Notes, dto.CalibrationId);

            await calibrationMeasurementRepository.AddAsync(calibrationMeasurement);

            dto.Id = calibrationMeasurement.Id;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await calibrationMeasurementRepository.DeleteAsync(id);
        }

        public async Task<CalibrationMeasurementDTO?> GetAsync(int id)
        {
            CalibrationMeasurement? calibrationMeasurement = await calibrationMeasurementRepository.GetAsync(id);

            if (calibrationMeasurement == null)
                return null;

            return MapToDto(calibrationMeasurement);
        }

        public async Task<IEnumerable<CalibrationMeasurementDTO>> GetAllAsync()
        {
            var calibrationMeasurements = await calibrationMeasurementRepository.GetAllAsync();
            return calibrationMeasurements.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(CalibrationMeasurementDTO dto)
        {
            var existing = await calibrationMeasurementRepository.GetAsync(dto.Id);
            if (existing == null)
                return false;

            await EnsureCalibrationExistsAsync(dto.CalibrationId);

            CalibrationMeasurement calibrationMeasurement = new CalibrationMeasurement(dto.Id, dto.NominalValue, dto.MeasuredValue, dto.Error, dto.IsWithinTolerance, dto.Notes, dto.CalibrationId);
            return await calibrationMeasurementRepository.UpdateAsync(calibrationMeasurement);
        }

        private async Task EnsureCalibrationExistsAsync(int calibrationId)
        {
            var calibration = await calibrationRepository.GetAsync(calibrationId);
            if (calibration == null)
                throw new KeyNotFoundException($"There is no calibration with id {calibrationId}.");
        }

        private static CalibrationMeasurementDTO MapToDto(CalibrationMeasurement calibrationMeasurement)
        {
            return new CalibrationMeasurementDTO
            {
                Id = calibrationMeasurement.Id,
                NominalValue = calibrationMeasurement.NominalValue,
                MeasuredValue = calibrationMeasurement.MeasuredValue,
                Error = calibrationMeasurement.Error,
                IsWithinTolerance = calibrationMeasurement.IsWithinTolerance,
                Notes = calibrationMeasurement.Notes,
                CalibrationId = calibrationMeasurement.CalibrationId
            };
        }
    }
}
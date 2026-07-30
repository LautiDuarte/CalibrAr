using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Instrument
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string? SerialNumber { get; private set; }
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
        public InstrumentStatus Status { get; private set; }
        public decimal? MaxAllowedError { get; private set; }
        public int? CalibrationFrequencyMonths { get; private set; }
        public DateTime? LastCalibrationDate { get; private set; }
        public DateTime? NextCalibrationDate { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private int _instrumentTypeId;
        private InstrumentType? _instrumentType;
        private int _areaId;
        private Area? _area;
        public int InstrumentTypeId
        {
            get => _instrumentType?.Id ?? _instrumentTypeId;
            private set => _instrumentTypeId = value;
        }
        public InstrumentType? InstrumentType
        {
            get => _instrumentType;
            private set
            {
                _instrumentType = value;
                _instrumentTypeId = value?.Id ?? _instrumentTypeId;
            }
        }
        public int AreaId
        {
            get => _area?.Id ?? _areaId;
            private set => _areaId = value;
        }
        public Area? Area
        {
            get => _area;
            private set
            {
                _area = value;
                _areaId = value?.Id ?? _areaId;
            }
        }

        public Instrument(int id, string code, string name, string? serialNumber, string? brand, string? model, InstrumentStatus status, decimal? maxAllowedError, int? calibrationFrequencyMonths, DateTime? lastCalibrationDate, DateTime? nextCalibrationDate, bool isActive, DateTime createdAt, DateTime? updatedAt, int instrumentTypeId, int areaId)
        {
            SetId(id);
            SetCode(code);
            SetName(name);
            SetSerialNumber(serialNumber);
            SetBrand(brand);
            SetModel(model);
            SetStatus(status);
            SetMaxAllowedError(maxAllowedError);
            SetCalibrationFrequencyMonths(calibrationFrequencyMonths);
            SetLastCalibrationDate(lastCalibrationDate);
            SetNextCalibrationDate(nextCalibrationDate);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
            SetUpdatedAt(updatedAt);
            SetInstrumentTypeId(instrumentTypeId);
            SetAreaId(areaId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código no puede ser nulo o vacío.", nameof(code));
            Code = code;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(name));
            Name = name;
        }

        public void SetSerialNumber(string? serialNumber)
        {
            SerialNumber = serialNumber;
        }

        public void SetBrand(string? brand)
        {
            Brand = brand;
        }

        public void SetModel(string? model)
        {
            Model = model;
        }

        public void SetStatus(InstrumentStatus status)
        {
            Status = status;
        }

        public void SetMaxAllowedError(decimal? maxAllowedError)
        {
            if (maxAllowedError < 0)
                throw new ArgumentException("El error máximo permitido no puede ser negativo.", nameof(maxAllowedError));
            MaxAllowedError = maxAllowedError;
        }

        public void SetCalibrationFrequencyMonths(int? calibrationFrequencyMonths)
        {
            if (calibrationFrequencyMonths < 0)
                throw new ArgumentException("La frecuencia de calibración no puede ser negativa.", nameof(calibrationFrequencyMonths));
            CalibrationFrequencyMonths = calibrationFrequencyMonths;
        }

        public void SetLastCalibrationDate(DateTime? lastCalibrationDate)
        {
            LastCalibrationDate = lastCalibrationDate;
        }

        public void SetNextCalibrationDate(DateTime? nextCalibrationDate)
        {
            NextCalibrationDate = nextCalibrationDate;
        }

        public void SetIsActive(bool isActive)
        {
            ArgumentNullException.ThrowIfNull(isActive);
            IsActive = isActive;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de creación no puede ser nula.", nameof(createdAt));
            CreatedAt = createdAt;
        }

        public void SetUpdatedAt(DateTime? updatedAt)
        {
            UpdatedAt = updatedAt;
        }

        public void SetInstrumentTypeId( int instrumentTypeId)
        {
            if (InstrumentTypeId <= 0)
                throw new ArgumentException("El Id del tipo de instrumento debe ser mayor que 0.", nameof(InstrumentTypeId));
            if (_instrumentType != null && _instrumentType.Id != instrumentTypeId)
                InstrumentType = null;
            InstrumentTypeId = instrumentTypeId;
        }

        public void SetAreaId(int areaId)
        {
            if (AreaId <= 0)
                throw new ArgumentException("El Id del área debe ser mayor que 0.", nameof(AreaId));
            if (_area != null && _area.Id != areaId)
                Area = null;
            AreaId = areaId;
        }

        public void SetInstrumentType(InstrumentType instrumentType)
        {
            ArgumentNullException.ThrowIfNull(instrumentType);
            InstrumentType = instrumentType;
        }

        public void SetArea(Area area)
        {
            ArgumentNullException.ThrowIfNull(area);
            Area = area;
        }
    }
}

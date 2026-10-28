using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class InstrumentType
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = String.Empty;
        public string? Description { get; private set; }
        public string MeasurementUnit { get; private set; } = String.Empty;
        public decimal MaxAllowedError { get; private set; }
        public int CalibrationFrequencyMonths { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public InstrumentType(int id, string name, string? description, string measurementUnit, decimal maxAllowedError, int calibrationFrequencyMonths, bool isActive, DateTime createdAt)
        {
            SetId(id);
            SetName(name);
            SetDescription(description);
            SetMeasurementUnit(measurementUnit);
            SetMaxAllowedError(maxAllowedError);
            SetCalibrationFrequencyMonths(calibrationFrequencyMonths);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El id debe ser mayor que 0.");
            Id = id;
        }
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.");
            Name = name;
        }

        public void SetDescription(string? description)
        {
            Description = description;
        }

        public void SetMeasurementUnit(string measurementUnit)
        {
            if (string.IsNullOrWhiteSpace(measurementUnit))
                throw new ArgumentException("La unidad de medida no puede estar vacía.");
            MeasurementUnit = measurementUnit;
        }

        public void SetMaxAllowedError(decimal maxAllowedError)
        {
            if (maxAllowedError <= 0)
            {
                throw new ArgumentException("El error máximo permitido debe ser mayor que 0.");
            }
            MaxAllowedError = maxAllowedError;
        }

        public void SetCalibrationFrequencyMonths(int calibrationFrequencyMonths)
        {
            if (calibrationFrequencyMonths <= 0)
            {
                throw new ArgumentException("La frecuencia de calibración debe ser mayor que 0.");
            }
            CalibrationFrequencyMonths = calibrationFrequencyMonths;
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

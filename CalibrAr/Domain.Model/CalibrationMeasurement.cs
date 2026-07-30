using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class CalibrationMeasurement
    {
        public int Id { get; private set; }
        public decimal NominalValue { get; private set; }
        public decimal MeasuredValue { get; private set; }
        public decimal Error { get; private set; }
        public bool IsWithinTolerance { get; private set; }
        public string? Notes { get; private set; }
        private int _calibrationId;
        private Calibration? _calibration;
        public int CalibrationId
        {
            get => _calibration?.Id ?? _calibrationId;
            private set => _calibrationId = value;
        }
        public Calibration? Calibration
        {
            get => _calibration;
            private set
            {
                _calibration = value;
                _calibrationId = value?.Id ?? _calibrationId;
            }
        }

        public CalibrationMeasurement(int id, decimal nominalValue, decimal measuredValue, decimal error, bool isWithinTolerance, string? notes, int calibrationId)
        {
            SetId(id);
            SetNominalValue(nominalValue);
            SetMeasuredValue(measuredValue);
            SetError(error);
            SetIsWithinTolerance(isWithinTolerance);
            SetNotes(notes);
            SetCalibrationId(calibrationId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }

        public void SetNominalValue(decimal nominalValue)
        {
            if (nominalValue <= 0)
                throw new ArgumentException("El valor nominal debe ser mayor que 0.", nameof(nominalValue));
            NominalValue = nominalValue;
        }

        public void SetMeasuredValue(decimal measuredValue)
        {
            if (measuredValue <= 0)
                throw new ArgumentException("El valor medido debe ser mayor que 0.", nameof(measuredValue));
            MeasuredValue = measuredValue;
        }

        public void SetError(decimal error)
        {
            Error = error;
        }

        public void SetIsWithinTolerance(bool isWithinTolerance)
        {
            IsWithinTolerance = isWithinTolerance;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }

        public void SetCalibrationId(int calibrationId)
        {
            if (calibrationId < 0)
                throw new ArgumentException("El Id de calibración debe ser mayor que 0.", nameof(calibrationId));
            if (_calibration != null && _calibration.Id != calibrationId)
                Calibration = null;
            CalibrationId = calibrationId;
        }

        public void SetCalibration(Calibration? calibration)
        {
            ArgumentNullException.ThrowIfNull(calibration, nameof(calibration));
            Calibration = calibration;

        }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class NonConformity
    {
        public int Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public Origin Origin { get; private set; }
        public NonConformityStatus Status { get; private set; }
        public string? CorrectiveAction { get; private set; }
        public DateTime OpenedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private int _instrumentId;
        private int _calibrationId;
        private int _detectedByUserId;
        private int _closedByUserId;
        private Instrument? _instrument;
        private Calibration? _calibration;
        private User? _detectedByUser;
        private User? _closedByUser;

        public int InstrumentId
        {
            get => _instrument?.Id ?? _instrumentId;
            private set => _instrumentId = value;
        }
        public int CalibrationId
        {
            get => _calibration?.Id ?? _calibrationId;
            private set => _calibrationId = value;
        }
        public int DetectedByUserId
        {
            get => _detectedByUser?.Id ?? _detectedByUserId;
            private set => _detectedByUserId = value;
        }
        public int ClosedByUserId
        {
            get => _closedByUser?.Id ?? _closedByUserId;
            private set => _closedByUserId = value;
        }
        public Instrument? Instrument
        {
            get => _instrument;
            private set
            {
                _instrument = value;
                _instrumentId = value?.Id ?? _instrumentId;
            }
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
        public User? DetectedByUser
        {
            get => _detectedByUser;
            private set
            {
                _detectedByUser = value;
                _detectedByUserId = value?.Id ?? _detectedByUserId;
            }
        }
        public User? ClosedByUser
        {
            get => _closedByUser;
            private set
            {
                _closedByUser = value;
                _closedByUserId = value?.Id ?? _closedByUserId;
            }
        }

        public NonConformity(int id, string code, string description, Origin origin, NonConformityStatus status, string? correctiveAction, DateTime openedAt, DateTime? closedAt, DateTime createdAt, int instrumentId, int calibrationId, int detectedByUserId, int closedByUserId)
        {
            SetId(id);
            SetCode(code);
            SetDescription(description);
            SetOrigin(origin);
            SetStatus(status);
            SetCorrectiveAction(correctiveAction);
            SetOpenedAt(openedAt);
            SetClosedAt(closedAt);
            SetCreatedAt(createdAt);
            SetInstrumentId(instrumentId);
            SetCalibrationId(calibrationId);
            SetDetectedByUserId(detectedByUserId);
            SetClosedByUserId(closedByUserId);
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
                throw new ArgumentException("El código no puede estar vacío.", nameof(code));
            Code = code;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(description));
            Description = description;
        }

        public void SetOrigin(Origin origin)
        {
            Origin = origin;
        }

        public void SetStatus(NonConformityStatus status)
        {
            Status = status;
        }

        public void SetCorrectiveAction(string? correctiveAction)
        {
            CorrectiveAction = correctiveAction;
        }

        public void SetOpenedAt(DateTime openedAt)
        {
            if (openedAt == default)
                throw new ArgumentException("La fecha de apertura no puede ser nula.", nameof(openedAt));
            OpenedAt = openedAt;
        }

        public void SetClosedAt(DateTime? closedAt)
        {
            ClosedAt = closedAt;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de creación no puede ser nula.", nameof(createdAt));
            CreatedAt = createdAt;
        }

        public void SetInstrumentId(int instrumentId)
        {
            if (instrumentId <= 0)
                throw new ArgumentException("El Id del instrumento debe ser mayor que 0.", nameof(instrumentId));
            if (_instrument != null && _instrument.Id != instrumentId)
                Instrument = null;
            InstrumentId = instrumentId;
        }

        public void SetCalibrationId(int calibrationId)
        {
            if (calibrationId <= 0)
                throw new ArgumentException("El Id de la calibración debe ser mayor que 0.", nameof(calibrationId));
            if (_calibration != null && _calibration.Id != calibrationId)
                Calibration = null;
            CalibrationId = calibrationId;
        }

        public void SetDetectedByUserId(int detectedByUserId)
        {
            if (detectedByUserId <= 0)
                throw new ArgumentException("El Id del usuario que detectó la no conformidad debe ser mayor que 0.", nameof(detectedByUserId));
            if (_detectedByUser != null && _detectedByUser.Id != detectedByUserId)
                DetectedByUser = null;
            DetectedByUserId = detectedByUserId;
        }

        public void SetClosedByUserId(int closedByUserId)
        {
            if (closedByUserId <= 0)
                throw new ArgumentException("El Id del usuario que cerró la no conformidad debe ser mayor que 0.", nameof(closedByUserId));
            if (_closedByUser != null && _closedByUser.Id != closedByUserId)
                ClosedByUser = null;
            ClosedByUserId = closedByUserId;
        }

        public void SetInstrument(Instrument instrument)
        {
            ArgumentNullException.ThrowIfNull(instrument);
            Instrument = instrument;
        }

        public void SetCalibration(Calibration calibration)
        {
            ArgumentNullException.ThrowIfNull(calibration);
            Calibration = calibration;
        }

        public void SetDetectedByUser(User detectedByUser)
        {
            ArgumentNullException.ThrowIfNull(detectedByUser);
            DetectedByUser = detectedByUser;
        }

        public void SetClosedByUser(User closedByUser)
        {
            ArgumentNullException.ThrowIfNull(closedByUser);
            ClosedByUser = closedByUser;
        }
    }
}
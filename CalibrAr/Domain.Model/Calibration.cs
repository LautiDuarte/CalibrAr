using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Calibration
    {
        public int Id { get; private set; }
        public DateTime CalibrationDate { get; private set; }
        public InterventionType InterventionType { get; private set; }
        public bool IsExternal { get; private set; }
        public string? ExternalLab { get; private set; }
        public string? CertificateNumber { get; private set; }
        public Result Result { get; private set; }
        public string? RestrictionDetail { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private int _instrumentId;
        private int? _procedureId;
        private int? _performedByUserId;
        private int? _approvedByUserId;
        private Instrument? _instrument;
        private Procedure? _procedure;
        private User? _performedByUser;
        private User? _approvedByUser;

        public int InstrumentId
        {
            get => _instrument?.Id ?? _instrumentId;
            private set => _instrumentId = value;
        }
        public int? ProcedureId
        {
            get => _procedure?.Id ?? _procedureId;
            private set => _procedureId = value;
        }
        public int? PerformedByUserId
        {
            get => _performedByUser?.Id ?? _performedByUserId;
            private set => _performedByUserId = value;
        }
        public int? ApprovedByUserId
        {
            get => _approvedByUser?.Id ?? _approvedByUserId;
            private set => _approvedByUserId = value;
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
        public Procedure? Procedure
        {
            get => _procedure;
            private set
            {
                _procedure = value;
                _procedureId = value?.Id ?? _procedureId;
            }
        }
        public User? PerformedByUser
        {
            get => _performedByUser;
            private set
            {
                _performedByUser = value;
                _performedByUserId = value?.Id ?? _performedByUserId;
            }
        }
        public User? ApprovedByUser
        {
            get => _approvedByUser;
            private set
            {
                _approvedByUser = value;
                _approvedByUserId = value?.Id ?? _approvedByUserId;
            }
        }

        public virtual ICollection<ReferenceStandard> ReferenceStandards { get; private set; } = new List<ReferenceStandard>();

        public virtual ICollection<CalibrationMeasurement> Measurements { get; private set; } = new List<CalibrationMeasurement>();

        public Calibration(int id, DateTime calibrationDate, InterventionType interventionType, bool isExternal, string? externalLab, string? certificateNumber, Result result, string? restrictionDetail, string? notes, DateTime createdAt, int instrumentId, int? procedureId, int? performedByUserId, int? approvedByUserId)
        {
            SetId(id);
            SetCalibrationDate(calibrationDate);
            SetInterventionType(interventionType);
            SetIsExternal(isExternal);
            SetExternalLab(externalLab);
            SetCertificateNumber(certificateNumber);
            SetResult(result);
            SetRestrictionDetail(restrictionDetail);
            SetNotes(notes);
            SetCreatedAt(createdAt);
            SetInstrumentId(instrumentId);
            SetProcedureId(procedureId);
            SetPerformedByUserId(performedByUserId);
            SetApprovedByUserId(approvedByUserId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El id debe ser mayor que 0.");
            Id = id;
        }

        public void SetCalibrationDate(DateTime calibrationDate)
        {
            if (calibrationDate > DateTime.Now)
                throw new ArgumentException("La fecha de calibración no puede ser futura.");
            if (calibrationDate == default)
                throw new ArgumentException("La fecha de calibración no puede estar vacía.");
            CalibrationDate = calibrationDate;
        }

        public void SetInterventionType(InterventionType interventionType)
        {
            InterventionType = interventionType;
        }

        public void SetIsExternal(bool isExternal)
        {
            IsExternal = isExternal;
        }

        public void SetExternalLab(string? externalLab)
        {
            ExternalLab = externalLab;
        }

        public void SetCertificateNumber(string? certificateNumber)
        {
            CertificateNumber = certificateNumber;
        }

        public void SetResult(Result result)
        {
            Result = result;
        }

        public void SetRestrictionDetail(string? restrictionDetail)
        {
            RestrictionDetail = restrictionDetail;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
                throw new ArgumentException("La fecha de creación no puede estar vacía.");
            CreatedAt = createdAt;
        }

        public void SetInstrumentId(int instrumentId)
        {
            if (instrumentId <= 0)
                throw new ArgumentException("El id del instrumento debe ser mayor que 0.");
            if (_instrument != null && _instrument.Id != instrumentId)
                Instrument = null;
            InstrumentId = instrumentId;
        }

        public void SetProcedureId(int? procedureId)
        {
            if (procedureId.HasValue && procedureId <= 0)
                throw new ArgumentException("El id del procedimiento debe ser mayor que 0.");
            if (_procedure != null && _procedure.Id != procedureId)
                Procedure = null;
            ProcedureId = procedureId;
        }

        public void SetPerformedByUserId(int? performedByUserId)
        {
            if (performedByUserId.HasValue && performedByUserId <= 0)
                throw new ArgumentException("El id del usuario que realizó la calibración debe ser mayor que 0.");
            if (_performedByUser != null && _performedByUser.Id != performedByUserId)
                PerformedByUser = null;
            PerformedByUserId = performedByUserId;
        }

        public void SetApprovedByUserId(int? approvedByUserId)
        {
            if (approvedByUserId.HasValue && approvedByUserId <= 0)
                throw new ArgumentException("El id del usuario que aprobó la calibración debe ser mayor que 0.");
            if (_approvedByUser != null && _approvedByUser.Id != approvedByUserId)
                ApprovedByUser = null;
            ApprovedByUserId = approvedByUserId;
        }

        public void SetInstrument(Instrument instrument)
        {
            ArgumentNullException.ThrowIfNull(instrument);
            Instrument = instrument;
        }

        public void SetProcedure(Procedure procedure)
        {
            ArgumentNullException.ThrowIfNull(procedure);
            Procedure = procedure;
        }

        public void SetPerformedByUser(User performedByUser)
        {
            ArgumentNullException.ThrowIfNull(performedByUser);
            PerformedByUser = performedByUser;
        }

        public void SetApprovedByUser(User approvedByUser)
        {
            ArgumentNullException.ThrowIfNull(approvedByUser);
            ApprovedByUser = approvedByUser;
        }

        public void SetMeasurements(IEnumerable<CalibrationMeasurement> measurements)
        {
            ArgumentNullException.ThrowIfNull(measurements);
            Measurements = measurements.ToList();
        }

        public void SetReferenceStandards(IEnumerable<ReferenceStandard> referenceStandards)
        {
            ArgumentNullException.ThrowIfNull(referenceStandards);
            ReferenceStandards = referenceStandards.ToList();
        }
    }
}

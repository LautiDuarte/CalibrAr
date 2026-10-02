using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Procedure
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string VersionNumber { get; private set; } = string.Empty;
        public DateTime ApprovedAt { get; private set; }

        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private int _instrumentTypeId;
        private InstrumentType? _instrumentType;
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

        public Procedure(int id, string code, string name, string versionNumber, DateTime approvedAt, bool isActive, DateTime createdAt, int instrumentTypeId)
        {
            SetId(id);
            SetCode(code);
            SetName(name);
            SetVersionNumber(versionNumber);
            SetApprovedAt(approvedAt);
            SetIsActive(isActive);
            SetCreatedAt(createdAt);
            SetInstrumentTypeId(instrumentTypeId);
        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            Id = id;
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("The code cannot be null or empty.", nameof(code));
            Code = code;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name cannot be null or empty.", nameof(name));
            Name = name;
        }

        public void SetVersionNumber(string versionNumber)
        {
            if (string.IsNullOrWhiteSpace(versionNumber))
                throw new ArgumentException("The version number cannot be null or empty.", nameof(versionNumber));
            VersionNumber = versionNumber;
        }

        public void SetApprovedAt(DateTime approvedAt)
        {
            if (approvedAt == default)
                throw new ArgumentException("The approval date cannot be null.", nameof(approvedAt));
            ApprovedAt = approvedAt;
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

        public void SetInstrumentTypeId(int instrumentTypeId)
        {
            if (instrumentTypeId <= 0)
                throw new ArgumentException("Instrmument type Id must be greater than 0.", nameof(instrumentTypeId));
            if (_instrumentType != null && _instrumentType.Id != instrumentTypeId)
                InstrumentType = null;
            InstrumentTypeId = instrumentTypeId;
        }

        public void SetInstrumentType(InstrumentType instrumentType)
        {
            ArgumentNullException.ThrowIfNull(instrumentType);
            InstrumentType = instrumentType;
        }
    }
}

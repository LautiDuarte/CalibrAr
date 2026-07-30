using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class InstrumentStatusHistory
    {
        public int Id { get; private set; }
        public InstrumentStatus PreviousStatus { get; private set; }
        public InstrumentStatus NewStatus { get; private set; }
        public string? Reason { get; private set; }
        public DateTime ChangedAt { get; private set; }
        private int _changedByUserId;
        private User? _changedByUser;
        public int ChangedByUserId
        {
            get => _changedByUser?.Id ?? _changedByUserId;
            private set => _changedByUserId = value;
        }
        public User? ChangedByUser
        {
            get => _changedByUser;
            private set
            {
                _changedByUser = value;
                _changedByUserId = value?.Id ?? _changedByUserId;
            }
        }
        private int _instrumentId;
        private Instrument? _instrument;
        public int InstrumentId
        {
            get => _instrument?.Id ?? _instrumentId;
            private set => _instrumentId = value;
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
        public InstrumentStatusHistory(int id, int instrumentId, InstrumentStatus previousStatus, InstrumentStatus newStatus, string? reason, int changedByUserId, DateTime changedAt)
        {
            SetId(id);
            SetPreviousStatus(previousStatus);
            SetNewStatus(newStatus);
            SetInstrumentId(instrumentId);
            SetReason(reason);
            SetChangedByUserId(changedByUserId);
            SetChangedAt(changedAt);

        }

        public void SetId(int id)
        {
            if (id < 0)
                throw new ArgumentException("El Id debe ser mayor que 0.", nameof(id));
            Id = id;
        }

        public void SetPreviousStatus(InstrumentStatus previousStatus)
        {
            PreviousStatus = previousStatus;
        }

        public void SetNewStatus(InstrumentStatus newStatus)
        {
            NewStatus = newStatus;
        }

        public void SetReason(string? reason)
        {
            Reason = reason;
        }

        public void SetChangedAt(DateTime changedAt)
        {
            if (changedAt == default)
                throw new ArgumentException("La fecha de cambio no puede ser nula.", nameof(changedAt));
            ChangedAt = changedAt;
        }

        public void SetChangedByUserId(int changedByUserId)
        {
            if (changedByUserId < 0)
                throw new ArgumentException("El Id del usuario que realizó el cambio debe ser mayor que 0.", nameof(changedByUserId));
            if (_changedByUser != null && _changedByUser.Id != changedByUserId)
                ChangedByUser = null; 
            ChangedByUserId = changedByUserId;
        }

        public void SetInstrumentId(int instrumentId)
        {
            if (instrumentId < 0)
                throw new ArgumentException("El Id del instrumento debe ser mayor que 0.", nameof(instrumentId));
            if (_instrument != null && _instrument.Id != instrumentId)
                Instrument = null; 
            InstrumentId = instrumentId;
        }

        public void SetChangedByUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ChangedByUser = user;
        }

        public void SetInstrument(Instrument instrument)
        {
            ArgumentNullException.ThrowIfNull(instrument, nameof(instrument));
            Instrument = instrument;
        }
    }

}

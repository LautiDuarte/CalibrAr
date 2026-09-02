using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public enum InstrumentStatusDTO
    {
        Active,
        TemporarilyOutOfService,
        CalibrationExpired,
        UnderRepair,
        Decommissioned,
        OnLoan
    }
}

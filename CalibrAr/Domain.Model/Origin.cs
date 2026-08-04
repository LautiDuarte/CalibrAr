using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public enum Origin
    {
        CalibrationExpired,
        NonConformantResult,
        PhysicalDamage,
        IncorrectUse,
        Other
    }
}

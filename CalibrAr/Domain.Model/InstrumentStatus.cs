using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public enum InstrumentStatus
    {
        Activo,
        Fuera_de_servicio_temporalmente,
        Calibracion_vencida,
        En_reparacion,
        Dado_de_baja,
        Prestado
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Application.Dtos
{
    public record RegistrarLeituraRequest(
         Guid TalhaoId,
         DateTime DataHoraLeituraUtc,
         decimal UmidadeSolo,
         decimal Temperatura,
         decimal Precipitacao
     );
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Application.Dtos
{
    public record LeituraSensorResponse(
     long Id,
     Guid TalhaoId,
     DateTime DataHoraLeituraUtc,
     decimal UmidadeSolo,
     decimal Temperatura,
     decimal Precipitacao
 );

    public record ConsultaLeiturasRequest(
        DateTime? DeUtc,
        DateTime? AteUtc,
        int? Limite
    );
}

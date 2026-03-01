using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingestion.Application.Dtos
{
    public record LeituraSensorRecebidaEvent(
        Guid TalhaoId,
        DateTime DataHoraLeituraUtc,
        decimal UmidadeSolo,
        decimal Temperatura,
        decimal Precipitacao
    );
}

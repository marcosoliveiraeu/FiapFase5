

namespace Telemetry.Application.Dtos
{
    public record LeituraSensorRecebidaEvent(
        Guid TalhaoId,
        DateTime DataHoraLeituraUtc,
        decimal UmidadeSolo,
        decimal Temperatura,
        decimal Precipitacao
    );
}

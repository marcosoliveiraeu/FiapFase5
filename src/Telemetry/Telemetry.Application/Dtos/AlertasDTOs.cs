
using Telemetry.Domain.Enums;

namespace Telemetry.Application.Dtos
{
    public record AlertaResponse(
        long Id,
        Guid TalhaoId,
        TipoAlerta Tipo,
        StatusAlerta Status,
        DateTime InicioUtc,
        DateTime? FimUtc,
        DateTime CriadoEmUtc,
        DateTime? FechadoEmUtc,
        string? Detalhes
    );

    public record ConsultaAlertasRequest(
        Guid? TalhaoId,
        StatusAlerta? Status,
        TipoAlerta? Tipo,
        DateTime? DeUtc,
        DateTime? AteUtc,
        int? Limite
    );
}

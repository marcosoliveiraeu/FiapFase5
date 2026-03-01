using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Application.Dtos;
using Telemetry.Application.Services.Interfaces;
using Telemetry.Domain.Enums;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly IConsultaTelemetriaService _service;

        public AlertasController(IConsultaTelemetriaService service) => _service = service;

        [HttpGet("alertas")]
        public Task<List<AlertaResponse>> Consultar(
            [FromQuery] Guid? talhaoId,
            [FromQuery] StatusAlerta? status,
            [FromQuery] TipoAlerta? tipo,
            [FromQuery] DateTime? deUtc,
            [FromQuery] DateTime? ateUtc,
            [FromQuery] int? limite,
            CancellationToken ct)
            => _service.ConsultarAlertasAsync(new ConsultaAlertasRequest(talhaoId, status, tipo, deUtc, ateUtc, limite), ct);

        [HttpGet("talhoes/{talhaoId:guid}/alertas")]
        public Task<List<AlertaResponse>> ConsultarPorTalhao(
            Guid talhaoId,
            [FromQuery] StatusAlerta? status,
            [FromQuery] TipoAlerta? tipo,
            [FromQuery] DateTime? deUtc,
            [FromQuery] DateTime? ateUtc,
            [FromQuery] int? limite,
            CancellationToken ct)
            => _service.ConsultarAlertasPorTalhaoAsync(talhaoId, new ConsultaAlertasRequest(talhaoId, status, tipo, deUtc, ateUtc, limite), ct);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Application.Dtos;
using Telemetry.Application.Services.Interfaces;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("talhoes/{talhaoId:guid}/leituras")]
    [Authorize]
    public class LeiturasController : ControllerBase
    {
        private readonly IConsultaTelemetriaService _service;

        public LeiturasController(IConsultaTelemetriaService service) => _service = service;

        [HttpGet]
        public Task<List<LeituraSensorResponse>> Consultar(
            Guid talhaoId,
            [FromQuery] DateTime? deUtc,
            [FromQuery] DateTime? ateUtc,
            [FromQuery] int? limite,
            CancellationToken ct)
            => _service.ConsultarLeiturasPorTalhaoAsync(talhaoId, new ConsultaLeiturasRequest(deUtc, ateUtc, limite), ct);
    }
}

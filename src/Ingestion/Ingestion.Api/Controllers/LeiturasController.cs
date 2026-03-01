using Ingestion.Application.Dtos;
using Ingestion.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ingestion.Api.Controllers
{
    [ApiController]
    [Route("leituras")]
    [Authorize]
    public class LeiturasController : ControllerBase
    {
        private readonly IIngestaoService _service;

        public LeiturasController(IIngestaoService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrarLeituraRequest request, CancellationToken ct)
        {
            await _service.IngerirAsync(request, ct);
            return Accepted();
        }
    }
}

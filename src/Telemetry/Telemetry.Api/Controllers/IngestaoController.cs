using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Application.Dtos;
using Telemetry.Application.Services.Interfaces;

namespace Telemetry.Api.Controllers
{
    [ApiController]
    [Route("leituras")]
    [Authorize] 
    public class IngestaoController : ControllerBase
    {
        private readonly IProcessadorLeituraService _processador;

        public IngestaoController(IProcessadorLeituraService processador)
            => _processador = processador;

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarLeituraRequest request, CancellationToken ct)
        {
            await _processador.ProcessarLeituraAsync(request, ct);
            return Accepted(); // 202 - processamento ok (mesmo que já tenha processado)
        }
    }
}

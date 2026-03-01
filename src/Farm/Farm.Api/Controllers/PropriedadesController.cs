using Farm.Application.DTOs;
using Farm.Application.Services.Interfaces;
using Identity.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farm.Api.Controllers
{
    [ApiController]
    [Route("propriedades")]
    [Authorize]
    public class PropriedadesController : Controller
    {
        private readonly IPropriedadeService _service;

        public PropriedadesController(IPropriedadeService service) => _service = service;

        [HttpGet]
        public Task<List<PropriedadeResponse>> ListarMinhas(CancellationToken ct)
            => _service.ListarMinhasAsync(User.ObterUsuarioId(), ct);

        [HttpGet("{id:guid}")]
        public Task<PropriedadeResponse> ObterMinha(Guid id, CancellationToken ct)
            => _service.ObterMinhaAsync(User.ObterUsuarioId(), id, ct);

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPropriedadeRequest request, CancellationToken ct)
        {
            var id = await _service.CriarAsync(User.ObterUsuarioId(), request, ct);
            return CreatedAtAction(nameof(ObterMinha), new { id }, new { id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarPropriedadeRequest request, CancellationToken ct)
        {
            await _service.AtualizarAsync(User.ObterUsuarioId(), id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover(Guid id, CancellationToken ct)
        {
            await _service.RemoverAsync(User.ObterUsuarioId(), id, ct);
            return NoContent();
        }
    }
}

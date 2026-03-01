using Farm.Application.DTOs;
using Farm.Application.Repositories;
using Farm.Application.Services.Interfaces;
using Farm.Domain.Entities;
using Identity.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farm.Api.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize]
    public class TalhoesController : ControllerBase
    {
        private readonly ITalhaoService _service;

        public TalhoesController(ITalhaoService service) => _service = service;

        [HttpGet("propriedades/{propriedadeId:guid}/talhoes")]
        public Task<List<TalhaoResponse>> Listar(Guid propriedadeId, CancellationToken ct) =>
            _service.ListarAsync(User.ObterUsuarioId(), propriedadeId, ct);

        [HttpPost("propriedades/{propriedadeId:guid}/talhoes")]
        public async Task<IActionResult> Criar(Guid propriedadeId, [FromBody] CriarTalhaoRequest request, CancellationToken ct)
        {
            var id = await _service.CriarAsync(User.ObterUsuarioId(), propriedadeId, request, ct);
            return Created($"/talhoes/{id}", new { id });
        }

        [HttpGet("talhoes/{id:guid}")]
        public Task<TalhaoResponse> Obter(Guid id, CancellationToken ct) =>
            _service.ObterAsync(User.ObterUsuarioId(), id, ct);

        [HttpPut("talhoes/{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarTalhaoRequest request, CancellationToken ct)
        {
            await _service.AtualizarAsync(User.ObterUsuarioId(), id, request, ct);
            return NoContent();
        }

        [HttpDelete("talhoes/{id:guid}")]
        public async Task<IActionResult> Remover(Guid id, CancellationToken ct)
        {
            await _service.RemoverAsync(User.ObterUsuarioId(), id, ct);
            return NoContent();
        }
    }
}

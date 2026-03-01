using Farm.Application.DTOs;
using Farm.Application.Exceptions;
using Farm.Application.Repositories;
using Farm.Application.Services.Interfaces;
using Farm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.Services
{
    public class PropriedadeService : IPropriedadeService
    {

        private readonly IPropriedadeRepository _repo;

        public PropriedadeService(IPropriedadeRepository repo) => _repo = repo;

        public async Task<List<PropriedadeResponse>> ListarMinhasAsync(Guid usuarioId, CancellationToken ct)
        {
            var props = await _repo.ListarPorUsuarioAsync(usuarioId, ct);
            return props.Select(p => new PropriedadeResponse(p.Id, p.Nome, p.Cidade, p.Estado, p.CriadoEmUtc)).ToList();
        }

        public async Task<PropriedadeResponse> ObterMinhaAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct)
        {
            var prop = await _repo.ObterPorIdAsync(propriedadeId, ct)
                       ?? throw new ApplicationFarmException("Propriedade não encontrada.");

            if (prop.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a esta propriedade.");

            return new PropriedadeResponse(prop.Id, prop.Nome, prop.Cidade, prop.Estado, prop.CriadoEmUtc);
        }

        public async Task<Guid> CriarAsync(Guid usuarioId, CriarPropriedadeRequest request, CancellationToken ct)
        {
            var prop = new Propriedade
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Nome = request.Nome,
                Cidade = request.Cidade,
                Estado = request.Estado,
                CriadoEmUtc = DateTime.UtcNow
            };

            await _repo.AdicionarAsync(prop, ct);
            await _repo.SalvarAsync(ct);

            return prop.Id;
        }

        public async Task AtualizarAsync(Guid usuarioId, Guid propriedadeId, AtualizarPropriedadeRequest request, CancellationToken ct)
        {
            var prop = await _repo.ObterPorIdAsync(propriedadeId, ct)
                       ?? throw new ApplicationFarmException("Propriedade não encontrada.");

            if (prop.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a esta propriedade.");

            prop.Nome = request.Nome;
            prop.Cidade = request.Cidade;
            prop.Estado = request.Estado;

            await _repo.SalvarAsync(ct);
        }

        public async Task RemoverAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct)
        {
            var prop = await _repo.ObterPorIdAsync(propriedadeId, ct)
                       ?? throw new ApplicationFarmException("Propriedade não encontrada.");

            if (prop.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a esta propriedade.");

            await _repo.RemoverAsync(prop, ct);
            await _repo.SalvarAsync(ct);
        }


    }
}

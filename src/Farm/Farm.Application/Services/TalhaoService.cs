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
    public class TalhaoService : ITalhaoService
    {
        private readonly IPropriedadeRepository _propriedades;
        private readonly ITalhaoRepository _talhoes;

        public TalhaoService(IPropriedadeRepository propriedades, ITalhaoRepository talhoes)
        {
            _propriedades = propriedades;
            _talhoes = talhoes;
        }

        public async Task<List<TalhaoResponse>> ListarAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct)
        {
            var prop = await _propriedades.ObterPorIdAsync(propriedadeId, ct)
                       ?? throw new ApplicationFarmException("Propriedade não encontrada.");

            if (prop.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a esta propriedade.");

            var itens = await _talhoes.ListarPorPropriedadeAsync(propriedadeId, ct);

            return itens.Select(t =>
                new TalhaoResponse(t.Id, t.PropriedadeId, t.Nome, t.Cultura, t.AreaHectares, t.CriadoEmUtc)
            ).ToList();
        }

        public async Task<TalhaoResponse> ObterAsync(Guid usuarioId, Guid talhaoId, CancellationToken ct)
        {
            var talhao = await _talhoes.ObterPorIdAsync(talhaoId, ct)
                         ?? throw new ApplicationFarmException("Talhão não encontrado.");

            // garante que talhão pertence ao usuário
            if (talhao.Propriedade.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a este talhão.");

            return new TalhaoResponse(talhao.Id, talhao.PropriedadeId, talhao.Nome, talhao.Cultura, talhao.AreaHectares, talhao.CriadoEmUtc);
        }

        public async Task<Guid> CriarAsync(Guid usuarioId, Guid propriedadeId, CriarTalhaoRequest request, CancellationToken ct)
        {
            var prop = await _propriedades.ObterPorIdAsync(propriedadeId, ct)
                       ?? throw new ApplicationFarmException("Propriedade não encontrada.");

            if (prop.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a esta propriedade.");

            var talhao = new Talhao
            {
                Id = Guid.NewGuid(),
                PropriedadeId = propriedadeId,
                Nome = request.Nome,
                Cultura = request.Cultura,
                AreaHectares = request.AreaHectares,
                CriadoEmUtc = DateTime.UtcNow
            };

            await _talhoes.AdicionarAsync(talhao, ct);
            await _talhoes.SalvarAsync(ct);

            return talhao.Id;
        }

        public async Task AtualizarAsync(Guid usuarioId, Guid talhaoId, AtualizarTalhaoRequest request, CancellationToken ct)
        {
            var talhao = await _talhoes.ObterPorIdAsync(talhaoId, ct)
                         ?? throw new ApplicationFarmException("Talhão não encontrado.");

            if (talhao.Propriedade.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a este talhão.");

            talhao.Nome = request.Nome;
            talhao.Cultura = request.Cultura;
            talhao.AreaHectares = request.AreaHectares;

            await _talhoes.SalvarAsync(ct);
        }

        public async Task RemoverAsync(Guid usuarioId, Guid talhaoId, CancellationToken ct)
        {
            var talhao = await _talhoes.ObterPorIdAsync(talhaoId, ct)
                         ?? throw new ApplicationFarmException("Talhão não encontrado.");

            if (talhao.Propriedade.UsuarioId != usuarioId)
                throw new ApplicationFarmException("Você não tem acesso a este talhão.");

            await _talhoes.RemoverAsync(talhao, ct);
            await _talhoes.SalvarAsync(ct);
        }
    }
}

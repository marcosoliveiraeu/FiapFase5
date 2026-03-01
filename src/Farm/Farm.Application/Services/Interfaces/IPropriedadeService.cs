using Farm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.Services.Interfaces
{
    public interface IPropriedadeService
    {
        Task<List<PropriedadeResponse>> ListarMinhasAsync(Guid usuarioId, CancellationToken ct);
        Task<PropriedadeResponse> ObterMinhaAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct);
        Task<Guid> CriarAsync(Guid usuarioId, CriarPropriedadeRequest request, CancellationToken ct);
        Task AtualizarAsync(Guid usuarioId, Guid propriedadeId, AtualizarPropriedadeRequest request, CancellationToken ct);
        Task RemoverAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct);
    }
}

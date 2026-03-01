using Farm.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.Services.Interfaces
{
    public interface ITalhaoService
    {
        Task<List<TalhaoResponse>> ListarAsync(Guid usuarioId, Guid propriedadeId, CancellationToken ct);
        Task<TalhaoResponse> ObterAsync(Guid usuarioId, Guid talhaoId, CancellationToken ct);

        Task<Guid> CriarAsync(Guid usuarioId, Guid propriedadeId, CriarTalhaoRequest request, CancellationToken ct);
        Task AtualizarAsync(Guid usuarioId, Guid talhaoId, AtualizarTalhaoRequest request, CancellationToken ct);
        Task RemoverAsync(Guid usuarioId, Guid talhaoId, CancellationToken ct);
    }
}

using Farm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.Repositories
{
    public interface IPropriedadeRepository
    {
        Task<List<Propriedade>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken ct);
        Task<Propriedade?> ObterPorIdAsync(Guid id, CancellationToken ct);
        Task AdicionarAsync(Propriedade propriedade, CancellationToken ct);
        Task RemoverAsync(Propriedade propriedade, CancellationToken ct);
        Task SalvarAsync(CancellationToken ct);
    }
}

using Farm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.Repositories
{
    public interface ITalhaoRepository
    {
        Task<List<Talhao>> ListarPorPropriedadeAsync(Guid propriedadeId, CancellationToken ct);
        Task<Talhao?> ObterPorIdAsync(Guid id, CancellationToken ct);
        Task AdicionarAsync(Talhao talhao, CancellationToken ct);
        Task RemoverAsync(Talhao talhao, CancellationToken ct);
        Task SalvarAsync(CancellationToken ct);
    }
}

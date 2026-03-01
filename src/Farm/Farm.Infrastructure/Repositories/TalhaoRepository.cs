using Farm.Application.Repositories;
using Farm.Domain.Entities;
using Farm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Infrastructure.Repositories
{
    public class TalhaoRepository : ITalhaoRepository
    {
        private readonly DbContextFarm _db;
        public TalhaoRepository(DbContextFarm db) => _db = db;

        public Task<List<Talhao>> ListarPorPropriedadeAsync(Guid propriedadeId, CancellationToken ct) =>
            _db.Talhoes.Where(t => t.PropriedadeId == propriedadeId).ToListAsync(ct);

        public Task<Talhao?> ObterPorIdAsync(Guid id, CancellationToken ct) =>
            _db.Talhoes.Include(t => t.Propriedade).FirstOrDefaultAsync(t => t.Id == id, ct);

        public Task AdicionarAsync(Talhao talhao, CancellationToken ct) =>
            _db.Talhoes.AddAsync(talhao, ct).AsTask();

        public Task RemoverAsync(Talhao talhao, CancellationToken ct)
        {
            _db.Talhoes.Remove(talhao);
            return Task.CompletedTask;
        }

        public Task SalvarAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}

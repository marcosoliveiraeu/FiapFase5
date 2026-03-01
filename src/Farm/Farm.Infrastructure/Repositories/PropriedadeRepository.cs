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
    public class PropriedadeRepository : IPropriedadeRepository
    {
        private readonly DbContextFarm _db;
        public PropriedadeRepository(DbContextFarm db) => _db = db;

        public Task<List<Propriedade>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken ct) =>
            _db.Propriedades.Where(p => p.UsuarioId == usuarioId).ToListAsync(ct);

        public Task<Propriedade?> ObterPorIdAsync(Guid id, CancellationToken ct) =>
            _db.Propriedades.Include(p => p.Talhoes).FirstOrDefaultAsync(p => p.Id == id, ct);

        public Task AdicionarAsync(Propriedade propriedade, CancellationToken ct) =>
            _db.Propriedades.AddAsync(propriedade, ct).AsTask();

        public Task RemoverAsync(Propriedade propriedade, CancellationToken ct)
        {
            _db.Propriedades.Remove(propriedade);
            return Task.CompletedTask;
        }

        public Task SalvarAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}

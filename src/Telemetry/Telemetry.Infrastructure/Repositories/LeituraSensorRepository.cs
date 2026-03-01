using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Application.Repositories;
using Telemetry.Domain.Entities;
using Telemetry.Infrastructure.Data;

namespace Telemetry.Infrastructure.Repositories
{
    public class LeituraSensorRepository : ILeituraSensorRepository
    {
        private readonly DbContextTelemetry _db;
        public LeituraSensorRepository(DbContextTelemetry db) => _db = db;

        public async Task<List<LeituraSensor>> ConsultarPorTalhaoAsync(
            Guid talhaoId,
            DateTime? deUtc,
            DateTime? ateUtc,
            int limite,
            CancellationToken ct)
        {
            var query = _db.LeiturasSensores
                .AsNoTracking()
                .Where(x => x.TalhaoId == talhaoId);

            if (deUtc.HasValue)
                query = query.Where(x => x.DataHoraLeituraUtc >= deUtc.Value);

            if (ateUtc.HasValue)
                query = query.Where(x => x.DataHoraLeituraUtc <= ateUtc.Value);

            return await query
                .OrderByDescending(x => x.DataHoraLeituraUtc)
                .Take(limite)
                .ToListAsync(ct);
        }

        public Task AdicionarAsync(LeituraSensor leitura, CancellationToken ct) =>
            _db.LeiturasSensores.AddAsync(leitura, ct).AsTask();

        public Task SalvarAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

        public Task<List<LeituraSensor>> ConsultarDesdeAsync(Guid talhaoId, DateTime desdeUtc, CancellationToken ct)
        {
            return _db.LeiturasSensores
                .AsNoTracking()
                .Where(x => x.TalhaoId == talhaoId && x.DataHoraLeituraUtc >= desdeUtc)
                .OrderBy(x => x.DataHoraLeituraUtc)
                .ToListAsync(ct);
        }

        public Task<LeituraSensor?> ObterUltimaAntesDeAsync(Guid talhaoId, DateTime antesDeUtc, CancellationToken ct)
        {
            return _db.LeiturasSensores
                .AsNoTracking()
                .Where(x => x.TalhaoId == talhaoId && x.DataHoraLeituraUtc < antesDeUtc)
                .OrderByDescending(x => x.DataHoraLeituraUtc)
                .FirstOrDefaultAsync(ct);
        }
    }
}

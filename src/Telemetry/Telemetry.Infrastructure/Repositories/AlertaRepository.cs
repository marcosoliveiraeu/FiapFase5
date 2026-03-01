using Microsoft.EntityFrameworkCore;
using Telemetry.Application.Repositories;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Enums;
using Telemetry.Infrastructure.Data;

namespace Telemetry.Infrastructure.Repositories
{
    public class AlertaRepository : IAlertaRepository
    {
        private readonly DbContextTelemetry _db;
        public AlertaRepository(DbContextTelemetry db) => _db = db;

        public async Task<List<Alerta>> ConsultarAsync(
            Guid? talhaoId,
            StatusAlerta? status,
            TipoAlerta? tipo,
            DateTime? deUtc,
            DateTime? ateUtc,
            int limite,
            CancellationToken ct)
        {
            var query = _db.Alertas.AsNoTracking().AsQueryable();

            if (talhaoId.HasValue)
                query = query.Where(x => x.TalhaoId == talhaoId.Value);

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (tipo.HasValue)
                query = query.Where(x => x.Tipo == tipo.Value);

            if (deUtc.HasValue)
                query = query.Where(x => x.CriadoEmUtc >= deUtc.Value);

            if (ateUtc.HasValue)
                query = query.Where(x => x.CriadoEmUtc <= ateUtc.Value);

            return await query
                .OrderByDescending(x => x.CriadoEmUtc)
                .Take(limite)
                .ToListAsync(ct);
        }

        public Task<Alerta?> ObterAlertaAbertoAsync(Guid talhaoId, TipoAlerta tipo, CancellationToken ct) =>
        _db.Alertas.FirstOrDefaultAsync(x =>
            x.TalhaoId == talhaoId &&
            x.Tipo == tipo &&
            x.Status == StatusAlerta.Aberto, ct);

        public Task AdicionarAsync(Alerta alerta, CancellationToken ct) =>
            _db.Alertas.AddAsync(alerta, ct).AsTask();

        public Task SalvarAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

    }
}

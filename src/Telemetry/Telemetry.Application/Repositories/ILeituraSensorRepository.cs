using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Entities;

namespace Telemetry.Application.Repositories
{
    public interface ILeituraSensorRepository
    {
        Task<List<LeituraSensor>> ConsultarPorTalhaoAsync(
            Guid talhaoId,
            DateTime? deUtc,
            DateTime? ateUtc,
            int limite,
            CancellationToken ct
        );

        Task<List<LeituraSensor>> ConsultarDesdeAsync(
            Guid talhaoId,
            DateTime desdeUtc,
            CancellationToken ct
        );

        Task<LeituraSensor?> ObterUltimaAntesDeAsync(
            Guid talhaoId,
            DateTime antesDeUtc,
            CancellationToken ct);

        Task AdicionarAsync(LeituraSensor leitura, CancellationToken ct);
        Task SalvarAsync(CancellationToken ct);
    }
}

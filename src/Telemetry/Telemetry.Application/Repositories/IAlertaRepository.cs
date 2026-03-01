using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Enums;

namespace Telemetry.Application.Repositories
{
    public interface IAlertaRepository
    {
        Task<List<Alerta>> ConsultarAsync(
            Guid? talhaoId,
            StatusAlerta? status,
            TipoAlerta? tipo,
            DateTime? deUtc,
            DateTime? ateUtc,
            int limite,
            CancellationToken ct
        );

        Task<Alerta?> ObterAlertaAbertoAsync(Guid talhaoId, TipoAlerta tipo, CancellationToken ct);

        Task AdicionarAsync(Alerta alerta, CancellationToken ct);
        Task SalvarAsync(CancellationToken ct);
    }
}

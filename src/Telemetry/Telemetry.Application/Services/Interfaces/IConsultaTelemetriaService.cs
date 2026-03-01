using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Application.Dtos;

namespace Telemetry.Application.Services.Interfaces
{
    public interface IConsultaTelemetriaService
    {
        Task<List<LeituraSensorResponse>> ConsultarLeiturasPorTalhaoAsync(Guid talhaoId, ConsultaLeiturasRequest request, CancellationToken ct);
        Task<List<AlertaResponse>> ConsultarAlertasAsync(ConsultaAlertasRequest request, CancellationToken ct);
        Task<List<AlertaResponse>> ConsultarAlertasPorTalhaoAsync(Guid talhaoId, ConsultaAlertasRequest request, CancellationToken ct);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Application.Dtos;
using Telemetry.Application.Repositories;
using Telemetry.Application.Services.Interfaces;

namespace Telemetry.Application.Services
{
    public class ConsultaTelemetriaService : IConsultaTelemetriaService
    {
        private readonly ILeituraSensorRepository _leituras;
        private readonly IAlertaRepository _alertas;

        public ConsultaTelemetriaService(ILeituraSensorRepository leituras, IAlertaRepository alertas)
        {
            _leituras = leituras;
            _alertas = alertas;
        }

        public async Task<List<LeituraSensorResponse>> ConsultarLeiturasPorTalhaoAsync(Guid talhaoId, ConsultaLeiturasRequest request, CancellationToken ct)
        {
            var limite = request.Limite is > 0 ? request.Limite.Value : 1000;

            var itens = await _leituras.ConsultarPorTalhaoAsync(
                talhaoId,
                request.DeUtc,
                request.AteUtc,
                limite,
                ct
            );

            return itens.Select(x => new LeituraSensorResponse(
                x.Id, x.TalhaoId, x.DataHoraLeituraUtc, x.UmidadeSolo, x.Temperatura, x.Precipitacao
            )).ToList();
        }


        public async Task<List<AlertaResponse>> ConsultarAlertasAsync(ConsultaAlertasRequest request, CancellationToken ct)
        {
            var limite = request.Limite is > 0 ? request.Limite.Value : 500;

            var itens = await _alertas.ConsultarAsync(
                request.TalhaoId,
                request.Status,
                request.Tipo,
                request.DeUtc,
                request.AteUtc,
                limite,
                ct
            );

            return itens.Select(x => new AlertaResponse(
                x.Id, x.TalhaoId, x.Tipo, x.Status, x.InicioUtc, x.FimUtc, x.CriadoEmUtc, x.FechadoEmUtc, x.Detalhes
            )).ToList();
        }

        public Task<List<AlertaResponse>> ConsultarAlertasPorTalhaoAsync(Guid talhaoId, ConsultaAlertasRequest request, CancellationToken ct)
        {
            var req = request with { TalhaoId = talhaoId };
            return ConsultarAlertasAsync(req, ct);
        }



    }
}

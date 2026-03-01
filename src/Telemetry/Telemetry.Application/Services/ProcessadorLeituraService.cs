using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Application.Dtos;
using Telemetry.Application.Repositories;
using Telemetry.Application.Services.Interfaces;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Enums;

namespace Telemetry.Application.Services
{
    public class ProcessadorLeituraService : IProcessadorLeituraService
    {
        private readonly ILeituraSensorRepository _leituras;
        private readonly IAlertaRepository _alertas;

        public ProcessadorLeituraService(ILeituraSensorRepository leituras, IAlertaRepository alertas)
        {
            _leituras = leituras;
            _alertas = alertas;
        }

        public async Task ProcessarLeituraAsync(RegistrarLeituraRequest request, CancellationToken ct)
        {
            // 1) Salva leitura
            var leitura = new LeituraSensor
            {
                TalhaoId = request.TalhaoId,
                DataHoraLeituraUtc = request.DataHoraLeituraUtc,
                UmidadeSolo = request.UmidadeSolo,
                Temperatura = request.Temperatura,
                Precipitacao = request.Precipitacao,
                RecebidoEmUtc = DateTime.UtcNow
            };

            await _leituras.AdicionarAsync(leitura, ct);
            await _leituras.SalvarAsync(ct);

            // 2) Avalia regras
            await AvaliarSecaAsync(request, ct);
            await AvaliarGeadaAsync(request, ct);
        }

        private async Task AvaliarSecaAsync(RegistrarLeituraRequest request, CancellationToken ct)
        {
            const decimal LIMIAR_SECA = 30m;
            var janelaInicio = request.DataHoraLeituraUtc.AddHours(-24);

            // leituras dentro da janela
            var leiturasJanela = await _leituras.ConsultarDesdeAsync(request.TalhaoId, janelaInicio, ct);

            // âncora anterior à janela
            var leituraAntes = await _leituras.ObterUltimaAntesDeAsync(request.TalhaoId, janelaInicio, ct);

            var alertaAberto = await _alertas.ObterAlertaAbertoAsync(request.TalhaoId, TipoAlerta.Seca, ct);

            // Condição para "seca por 24h":
            // - já estava abaixo do limiar antes de iniciar a janela
            // - e não houve nenhuma leitura >= limiar dentro da janela
            // - e a leitura atual ainda está abaixo
            var temAncoraSeca = leituraAntes is not null && leituraAntes.UmidadeSolo < LIMIAR_SECA;
            var janelaTodaSeca = leiturasJanela.Count > 0 && leiturasJanela.All(x => x.UmidadeSolo < LIMIAR_SECA);
            var leituraAtualSeca = request.UmidadeSolo < LIMIAR_SECA;

            if (temAncoraSeca && janelaTodaSeca && leituraAtualSeca)
            {
                if (alertaAberto is null)
                {
                    var alerta = new Alerta
                    {
                        TalhaoId = request.TalhaoId,
                        Tipo = TipoAlerta.Seca,
                        Status = StatusAlerta.Aberto,
                        InicioUtc = leituraAntes!.DataHoraLeituraUtc, 
                        CriadoEmUtc = DateTime.UtcNow,
                        Detalhes = "Umidade do solo < 30% mantida por pelo menos 24h."
                    };

                    await _alertas.AdicionarAsync(alerta, ct);
                    await _alertas.SalvarAsync(ct);
                }
            }
            else
            {
                //  se existir alerta aberto e a leitura atual normalizou, fecha
                if (alertaAberto is not null && request.UmidadeSolo >= LIMIAR_SECA)
                {
                    alertaAberto.Status = StatusAlerta.Fechado;
                    alertaAberto.FimUtc = request.DataHoraLeituraUtc;
                    alertaAberto.FechadoEmUtc = DateTime.UtcNow;

                    await _alertas.SalvarAsync(ct);
                }
            }
        }

        private async Task AvaliarGeadaAsync(RegistrarLeituraRequest request, CancellationToken ct)
        {
            const decimal LIMIAR_ABRE = 2m;   // <= 2°C por 2h
            const decimal LIMIAR_FECHA = 4m;  // fecha quando >= 4°C 
            var janelaInicio = request.DataHoraLeituraUtc.AddHours(-2);

            var leiturasJanela = await _leituras.ConsultarDesdeAsync(request.TalhaoId, janelaInicio, ct);
            var leituraAntes = await _leituras.ObterUltimaAntesDeAsync(request.TalhaoId, janelaInicio, ct);

            var alertaAberto = await _alertas.ObterAlertaAbertoAsync(request.TalhaoId, TipoAlerta.Geada, ct);

            var temAncoraFrio = leituraAntes is not null && leituraAntes.Temperatura <= LIMIAR_ABRE;
            var janelaTodaFria = leiturasJanela.Count > 0 && leiturasJanela.All(x => x.Temperatura <= LIMIAR_ABRE);
            var leituraAtualFria = request.Temperatura <= LIMIAR_ABRE;

            if (temAncoraFrio && janelaTodaFria && leituraAtualFria)
            {
                if (alertaAberto is null)
                {
                    var alerta = new Alerta
                    {
                        TalhaoId = request.TalhaoId,
                        Tipo = TipoAlerta.Geada,
                        Status = StatusAlerta.Aberto,
                        InicioUtc = leituraAntes!.DataHoraLeituraUtc,
                        CriadoEmUtc = DateTime.UtcNow,
                        Detalhes = "Temperatura <= 2°C mantida por pelo menos 2h."
                    };

                    await _alertas.AdicionarAsync(alerta, ct);
                    await _alertas.SalvarAsync(ct);
                }
            }
            else
            {
                // só fecha quando do limite de abertura para evitar oscilações
                if (alertaAberto is not null && request.Temperatura >= LIMIAR_FECHA)
                {
                    alertaAberto.Status = StatusAlerta.Fechado;
                    alertaAberto.FimUtc = request.DataHoraLeituraUtc;
                    alertaAberto.FechadoEmUtc = DateTime.UtcNow;

                    await _alertas.SalvarAsync(ct);
                }
            }
        }


       
    }
}

using Ingestion.Application.Dtos;
using Ingestion.Application.Mensageria;
using Ingestion.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingestion.Application.Services
{
    public class IngestaoService : IIngestaoService
    {
        private readonly ILeituraPublisher _publisher;

        public IngestaoService(ILeituraPublisher publisher) => _publisher = publisher;

        public Task IngerirAsync(RegistrarLeituraRequest request, CancellationToken ct)
        {
            var evt = new LeituraSensorRecebidaEvent(
                request.TalhaoId,
                request.DataHoraLeituraUtc,
                request.UmidadeSolo,
                request.Temperatura,
                request.Precipitacao
            );

            return _publisher.PublicarAsync(evt, ct);
        }
    }
}

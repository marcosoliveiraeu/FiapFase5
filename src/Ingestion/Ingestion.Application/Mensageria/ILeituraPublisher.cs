using Ingestion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingestion.Application.Mensageria
{
    public interface ILeituraPublisher
    {
        Task PublicarAsync(LeituraSensorRecebidaEvent evento, CancellationToken ct);
    }
}

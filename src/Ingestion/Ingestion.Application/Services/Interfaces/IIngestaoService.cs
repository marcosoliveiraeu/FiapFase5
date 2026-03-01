using Ingestion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingestion.Application.Services.Interfaces
{
    public interface IIngestaoService
    {
        Task IngerirAsync(RegistrarLeituraRequest request, CancellationToken ct);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Application.Dtos;

namespace Telemetry.Application.Services.Interfaces
{
    public interface IProcessadorLeituraService
    {
        Task ProcessarLeituraAsync(RegistrarLeituraRequest request, CancellationToken ct);
    }
}

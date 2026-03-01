using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.DTOs
{
    public record CriarTalhaoRequest(string Nome, string? Cultura, decimal? AreaHectares);
    public record AtualizarTalhaoRequest(string Nome, string? Cultura, decimal? AreaHectares);

    public record TalhaoResponse(
        Guid Id,
        Guid PropriedadeId,
        string Nome,
        string? Cultura,
        decimal? AreaHectares,
        DateTime CriadoEmUtc
    );
}

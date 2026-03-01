using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Application.DTOs
{
    public record CriarPropriedadeRequest(string Nome, string? Cidade, string? Estado);
    public record AtualizarPropriedadeRequest(string Nome, string? Cidade, string? Estado);

    public record PropriedadeResponse(
        Guid Id,
        string Nome,
        string? Cidade,
        string? Estado,
        DateTime CriadoEmUtc
    );
}

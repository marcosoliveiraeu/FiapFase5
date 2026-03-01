using Identity.Application.DTOs;
using Identity.Domain.Entities;


namespace Identity.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> VerificaEmailDupplicado(string email);

        Task CadastrarUsuarioAsync(string email, string senha);

        bool SenhaValida(string senha);

        Task<IEnumerable<Usuario>> ObterTodosAsync();

        Task AtualizarAsync(AtualizarUsuarioDto usuario);

        Task RemoverAsync(Guid id);

        Task<Usuario?> ObterPorIdAsync(Guid id);
    }
}

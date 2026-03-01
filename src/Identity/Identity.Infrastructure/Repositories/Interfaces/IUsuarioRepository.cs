using Identity.Domain.Entities;


namespace Identity.Infrastructure.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<bool> EmailExisteAsync(string email);
        Task IncluirAsync(Usuario usuario);
        Task<Usuario> GetByEmailAsync(string email);

        Task<Usuario> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Usuario>> ObterTodosAsync();

        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(Guid id);


    }
}

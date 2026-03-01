using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Identity.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbContextIdentity _dbContext;

        public UsuarioRepository(DbContextIdentity dbContextFCG)
        {
            _dbContext = dbContextFCG;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _dbContext.Usuarios.AsNoTracking().ToListAsync();
        }

        public async Task<bool> EmailExisteAsync(string email)
        {

            return await _dbContext.Usuarios.AnyAsync(u => u.Email == email);

        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {

            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        }

        public async Task IncluirAsync(Usuario usuario)
        {

            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<Usuario> ObterPorIdAsync(Guid id)
        {

            return await _dbContext.Usuarios.FindAsync(id);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _dbContext.Usuarios.Update(usuario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var usuario = await ObterPorIdAsync(id);
            if (usuario != null)
            {
                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}

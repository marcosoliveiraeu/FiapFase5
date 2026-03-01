using Identity.Application.DTOs;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Identity.Infrastructure.Repositories.Interfaces;


namespace Identity.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenhaHasher _senhaHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, ISenhaHasher senhaHasher)
        {
            _usuarioRepository = usuarioRepository;
            _senhaHasher = senhaHasher;
        }

        public async Task CadastrarUsuarioAsync(string email, string senha)
        {
            if (await _usuarioRepository.EmailExisteAsync(email))
                throw new BusinessException("Já existe um usuário cadastrado com esse email.");

            if (!SenhaValida(senha))
                throw new BusinessException("A senha não atende aos critérios de segurança.");

            Usuario usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = _senhaHasher.Hash(senha)
            };


            await _usuarioRepository.IncluirAsync(usuario);
        }

        public async Task<bool> VerificaEmailDupplicado(string email)
        {
            return await _usuarioRepository.EmailExisteAsync(email);

        }

        public bool SenhaValida(string senha)
        {
            if (senha.Length < 8)
                return false;

            bool temLetraMaiuscula = senha.Any(char.IsUpper);
            bool temLetraMinuscula = senha.Any(char.IsLower);
            bool temNumero = senha.Any(char.IsDigit);
            bool temCaractereEspecial = senha.Any(c => !char.IsLetterOrDigit(c));

            return temLetraMaiuscula && temLetraMinuscula && temNumero && temCaractereEspecial;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return usuarios;
        }


        public async Task RemoverAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
            {
                throw new ApplicationException("Usuario não encontrado.");
            }

            await _usuarioRepository.RemoverAsync(id);
        }

        public async Task AtualizarAsync(AtualizarUsuarioDto usuarioAlterado)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioAlterado.Id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            if (!SenhaValida(usuarioAlterado.PasswordHash))
                throw new BusinessException("A senha não atende aos critérios de segurança.");


            usuario.Email = usuarioAlterado.Email;
            usuario.PasswordHash = _senhaHasher.Hash(usuarioAlterado.PasswordHash);

            await _usuarioRepository.AtualizarAsync(usuario);


        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return usuario;

        }
    }
}

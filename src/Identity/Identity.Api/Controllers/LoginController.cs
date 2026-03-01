using Identity.Api.Utils;
using Identity.Application.DTOs;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;
        private readonly ISenhaHasher _senhaHasher;

        public LoginController(IUsuarioRepository usuarioRepository, TokenService tokenService, ISenhaHasher senhaHasher)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _senhaHasher = senhaHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            Usuario usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

            if (usuario == null || usuario.PasswordHash != _senhaHasher.Hash(request.Senha))
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            LoginResponseDto response = new LoginResponseDto
            {
                Token = _tokenService.GenerateToken(usuario)
            };


            return Ok(response);
        }

        [HttpGet("autenticado")]
        [Authorize]
        public IActionResult TesteAutenticado()
        {
            return Ok($"Acesso autenticado: usuário {User.Identity?.Name} está autenticado.");
        }
    }
}

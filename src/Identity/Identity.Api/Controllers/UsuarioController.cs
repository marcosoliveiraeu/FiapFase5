using Identity.Application.DTOs;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {

        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        ///     Cadastrar novo usuário 
        /// </summary>
        /// <remarks>
        ///     Cadastra um novo usuário 
        /// </remarks>
        [Authorize]
        [HttpPost("Registrar")]
        public async Task<ActionResult> Cadastrar(RegistrarUsuarioDto usuario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _usuarioService.CadastrarUsuarioAsync(usuario.Email, usuario.Senha);
                return Ok(new { message = "Usuário cadastrado com sucesso!" });
            }
            catch (BusinessException ex)
            {
                // Retorna uma mensagem amigável para o usuário
                return BadRequest(new { mensagem = ex.Message });
            }

        }

        /// <summary>
        /// Listar todos os usuários
        /// </summary>
        [HttpGet("BuscarTodos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();

            return Ok(usuarios);
        }

        /// <summary>
        /// Atualizar usuario
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id da URL diferente do body.");


            await _usuarioService.AtualizarAsync(dto);

            var response = new
            {
                Mensagem = "Usuário atualizado com sucesso.",
                Id = dto.Id,
                Email = dto.Email
            };


            return Ok(response);
        }

        /// <summary>
        /// Remover usuário
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");

            await _usuarioService.RemoverAsync(id);

            var response = new
            {
                Mensagem = "Usuário excluído com sucesso!",
                Id = usuario.Id,
                Email = usuario.Email
            };

            return Ok(response);
        }

        /// <summary>
        /// Obter usuário por ID
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObterPorId(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            return Ok(usuario);
        }
    }
}

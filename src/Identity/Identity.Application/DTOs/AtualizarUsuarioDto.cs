

using System.ComponentModel.DataAnnotations;

namespace Identity.Application.DTOs
{
    public class AtualizarUsuarioDto
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(200, ErrorMessage = "O hash da senha deve ter no máximo 200 caracteres.")]
        public string PasswordHash { get; set; }

    }
}

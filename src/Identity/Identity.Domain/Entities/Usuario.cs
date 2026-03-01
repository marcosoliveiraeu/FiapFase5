using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.Entities;

public class Usuario
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(200, ErrorMessage = "O hash da senha deve ter no máximo 200 caracteres.")]
    public string PasswordHash { get; set; } = null!;
}

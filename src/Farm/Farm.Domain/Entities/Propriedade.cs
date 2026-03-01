using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Domain.Entities
{
    public class Propriedade
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; } 

        [Required]
        [StringLength(120)]
        public string Nome { get; set; } = null!;

        [StringLength(120)]
        public string? Cidade { get; set; }

        [StringLength(2)]
        public string? Estado { get; set; }

        public DateTime CriadoEmUtc { get; set; } = DateTime.UtcNow;


        public ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
    }
}

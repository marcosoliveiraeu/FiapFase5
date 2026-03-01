using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Domain.Entities
{
    public class Talhao
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PropriedadeId { get; set; }

        [ForeignKey(nameof(PropriedadeId))]
        public Propriedade Propriedade { get; set; } = null!;

        [Required]
        [StringLength(120)]
        public string Nome { get; set; } = null!;

        [StringLength(80)]
        public string? Cultura { get; set; }

        public decimal? AreaHectares { get; set; }

        public DateTime CriadoEmUtc { get; set; } = DateTime.UtcNow;
    }
}

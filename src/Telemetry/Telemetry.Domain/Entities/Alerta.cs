using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Enums;

namespace Telemetry.Domain.Entities
{
    public class Alerta
    {
        [Key]
        public long Id { get; set; } 

        [Required]
        public Guid TalhaoId { get; set; }

        [Required]
        public TipoAlerta Tipo { get; set; }

        [Required]
        public StatusAlerta Status { get; set; } = StatusAlerta.Aberto;

        /// <summary>
        /// Momento em que a condição do alerta começou (ex.: início do período de seca).
        /// </summary>
        [Required]
        public DateTime InicioUtc { get; set; }

        /// <summary>
        /// Momento em que a condição deixou de existir (se fechado).
        /// </summary>
        public DateTime? FimUtc { get; set; }

        [Required]
        public DateTime CriadoEmUtc { get; set; } = DateTime.UtcNow;

        public DateTime? FechadoEmUtc { get; set; }

        [StringLength(2000)]
        public string? Detalhes { get; set; }
    }
}

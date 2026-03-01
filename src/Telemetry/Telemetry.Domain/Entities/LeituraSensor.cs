using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Domain.Entities
{
    public class LeituraSensor
    {
        [Key]
        public long Id { get; set; }  

        [Required]
        public Guid TalhaoId { get; set; } 

        [Required]
        public DateTime DataHoraLeituraUtc { get; set; } 

        [Required]
        [Range(0, 100)]
        public decimal UmidadeSolo { get; set; } // %

        [Required]
        public decimal Temperatura { get; set; } // °C

        [Required]
        public decimal Precipitacao { get; set; } // mm 

        [Required]
        public DateTime RecebidoEmUtc { get; set; } = DateTime.UtcNow;
    }
}

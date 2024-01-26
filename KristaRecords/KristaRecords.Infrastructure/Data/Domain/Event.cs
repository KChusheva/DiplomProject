using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Domain
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string EventName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        [Required]
        public string Image { get; set; } = null!;

        [Required]
        public DateTime CompletionDate { get; set; }
    }
}


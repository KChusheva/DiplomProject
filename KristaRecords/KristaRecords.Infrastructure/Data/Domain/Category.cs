using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Domain
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } = null!;

        [Required]
        public decimal HourlyRate { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    }
}

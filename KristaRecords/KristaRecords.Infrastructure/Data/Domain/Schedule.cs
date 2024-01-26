using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Domain
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AvailableHours { get; set; }

        [Required]
        public decimal Discount { get; set; }

        public bool IsBusy { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

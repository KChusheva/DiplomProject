using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Domain
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; } = null!;

        [Required]
        public int DurationHours { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime SubmissionDateTime { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        [Required]
        public decimal Discount { get; set; }


        public decimal TotalAmount
        {
            get
            {
                return this.HourlyRate * this.DurationHours - this.HourlyRate * this.DurationHours * this.Discount / 100;
            }
        }
    }
}

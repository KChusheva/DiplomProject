using KristaRecords.Models.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KristaRecords.Models.Reservation
{
    public class ReservationCreateVM
    {
        [Key]
        public int Id { get; set; }

        public string? ScheduleDate { get; set; }

        [Required(ErrorMessage = "Cannot have reservation without FROM date")]
        [Display(Name = "From: ")]
        public string? FromHour { get; set; }

        [Required(ErrorMessage = "Cannot have reservation without TO date")]
        [Display(Name = "To: ")]
        public string? ToHour { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration cannot be less than 1 hour")]
        public int Duration { get; set; }
        public decimal Discount { get; set; }

        [Display(Name = "Hours available")]
        public int AvailableHours { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total Price cannot be less than 1")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public List<(int, int)>? ReservationsDates { get; set; }
    }
}

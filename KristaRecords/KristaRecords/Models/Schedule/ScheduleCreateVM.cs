using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Schedule
{
    public class ScheduleCreateVM
    {
        [Required]
        public string? Date { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name = "Available Hours")]
        public int AvailableHours { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Discount { get; set; }
    }
}

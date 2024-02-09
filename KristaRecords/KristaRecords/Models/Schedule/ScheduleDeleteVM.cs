using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Schedule
{
    public class ScheduleDeleteVM
    {
        [Key]
        public int Id { get; set; }
        public string? Date { get; set; }

        
        [Display(Name = "Available Hours")]
        public int AvailableHours { get; set; }

        public decimal Discount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Schedule
{
    public class ScheduleIndexVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Available Date")]
        public string? Date { get; set; }
        [Display(Name = "Availavle Hours")]
        public int AvailableHours { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Studio is busy?")]
        public bool IsBusy { get; set; }
    }
}

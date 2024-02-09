using KristaRecords.Models.Category;
using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Event
{
    public class EventDetailsVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; } = null!;

        [Required]
        [Display(Name = "Day of Completion")]
        public string CompletionDate { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Price Per Hour")]
        public decimal CategoryHourlyRate { get; set; }
    }
}

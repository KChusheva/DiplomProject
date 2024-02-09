using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KristaRecords.Models.Event
{
    public class EventIndexVM
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = null!;

        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Display(Name = "Image")]
        public string Image { get; set; } = null!;

        [Display(Name = "Completion Date")]
        public string CompletionDate { get; set; } = null!;
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Price Per Hour")]
        public decimal CategoryHourlyRate { get; set; }
    }
}

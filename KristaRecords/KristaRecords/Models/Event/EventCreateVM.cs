using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Linq;
using KristaRecords.Models.Category;

namespace KristaRecords.Models.Event
{
    public class EventCreateVM
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
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();
    }
}

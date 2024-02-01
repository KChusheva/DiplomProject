using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KristaRecords.Models.Category
{
    public class CategoryPairVM
    {
        public int Id { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; } = null!;

        [Display(Name="Price Per Hour")]
        public decimal HourlyRate { get; set; }
    }
}

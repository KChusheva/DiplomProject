using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Infrastructure.Data.Domain
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;
        public virtual ApplicationUser? Author { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Content { get; set; }

        public int? ParentId { get; set; }
        public virtual Comment? Parent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public virtual IEnumerable<Comment>? Comments { get; set; }
    }
}

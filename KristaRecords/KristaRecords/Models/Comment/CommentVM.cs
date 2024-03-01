using KristaRecords.Infrastructure.Data.Domain;
using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Comment
{
    public class CommentVM
    {
        public IEnumerable<Infrastructure.Data.Domain.Comment>? Comments { get; set; }
        public Infrastructure.Data.Domain.Comment? AddComment { get; set; }
        public Infrastructure.Data.Domain.Comment? EditComment { get; set; }
    }
}

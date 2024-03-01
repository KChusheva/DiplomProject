using KristaRecords.Infrastructure.Data.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface IReviewService
    {
        IEnumerable<Comment> GetComments();
        Task<Comment> GetComment(int commentId);
        Task<ActionResult<Comment>> Add(Comment comment, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<Comment>> Edit(Comment comment, ClaimsPrincipal claimsPrincipal);
        Task DeleteComment(Comment comment);
    }
}

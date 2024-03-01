using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ReviewService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public async Task<Comment> GetComment(int commentId)
        {
            return await _context.Comments
                           .Include(comment => comment.Author)
                           .Include(comment => comment.Parent)
                           .FirstOrDefaultAsync(comment => comment.Id == commentId);
        }

        public IEnumerable<Comment> GetComments()
        {
            return _context.Comments
                           .Include(comment => comment.Author)
                           .Include(comment => comment.Parent);
        }

        public async Task<ActionResult<Comment>> Add(Comment comment, ClaimsPrincipal claimsPrincipal)
        {
            if(comment is null)
            {
                return new BadRequestResult();
            }

            var addComment = comment;
            addComment.Author = await _userManager.GetUserAsync(claimsPrincipal);
            addComment.CreatedOn = DateTime.Now;

            if(addComment.Parent != null)
            {
                addComment.Parent = await GetComment(addComment.Parent.Id);
            }

            _context.Add(addComment);
            await _context.SaveChangesAsync();
            return addComment;
        }

        public async Task<ActionResult<Comment>> Edit(Comment comment, ClaimsPrincipal claimsPrincipal)
        {
            if (comment is null)
            {
                return new BadRequestResult();
            }

            var editComment = await GetComment(comment.Id); ;
            editComment.Content = comment.Content;
            editComment.CreatedOn = DateTime.Now;

            _context.Update(editComment);
            await _context.SaveChangesAsync();
            return editComment;
        }

        public async Task DeleteComment(Comment comment)
        {
            if(comment.Comments.ToList().Count > 0)
            {
                foreach (var reply in comment.Comments)
                {
                    _context.Comments.Remove(reply);
                }
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}

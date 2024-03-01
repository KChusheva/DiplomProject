using KristaRecords.Core.Contracts;
using KristaRecords.Core.Services;
using KristaRecords.Infrastructure.Migrations;
using KristaRecords.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KristaRecords.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
  
        public ReviewController(IReviewService reviewService)
        {
            this._reviewService = reviewService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var comments = _reviewService.GetComments();

            var commentsVM = new CommentVM
            {
                Comments = comments
            };

            return View(commentsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CommentVM commentVM)
        {
            var created = await _reviewService.Add(commentVM.AddComment, User);

            return RedirectToAction("Index", "Review");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentVM commentVM)
        {
            var edited = await _reviewService.Edit(commentVM.EditComment, User);

            return RedirectToAction("Index", "Review");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _reviewService.GetComment(id);

            if (comment == null)
            {
                this.NotFound();
            }

            await _reviewService.DeleteComment(comment);

            return this.RedirectToAction(nameof(Index));
        }
    }
}

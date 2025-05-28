using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Review;

namespace RestrantApplication.MVC.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IUserContextService _userContextService;

        public ReviewController(IReviewService reviewService, IUserContextService userContextService)
        {
            _reviewService = reviewService;
            _userContextService = userContextService;
        }

        // GET: Display the review submission form
        [HttpGet]
        public IActionResult AddReview()
        {
            return View();
        }

        // POST: Handle review submission from the user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(AddReveiwViewModel addReviewViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userContextService.GetCurrentUserId();
                var done = await _reviewService.AddUserReviewAsync(userId, addReviewViewModel.Comment);
                if (done)
                {
                    return RedirectToAction("GetAllReviewsForCurrentUser");
                }
                ModelState.AddModelError("", "Failed to add review.");
            }
            return View(addReviewViewModel);
        }

        // GET: Display all reviews for the current user
        public async Task<IActionResult> GetAllReviewsForCurrentUser()
        {
            var userId = _userContextService.GetCurrentUserId();
            var reviews = await _reviewService.GetReviewsForUserAsync(userId);
            return View(reviews);
        }

        // GET: Delete a specific review (must belong to the current user)
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = _userContextService.GetCurrentUserId();

            // Optional: validate the review belongs to the user here
            var deleted = await _reviewService.DeleteUserReviewAsync(reviewId, userId);
            if (!deleted)
            {
                TempData["Error"] = "Failed to delete the review.";
            }

            return RedirectToAction("GetAllReviewsForCurrentUser");
        }

        // GET: Display the review update form for a given review
        [HttpGet]
        public async Task<IActionResult> UpdateReview(int reviewId)
        {
            var review = await _reviewService.GetReviewByIDAsync(reviewId);
            if (review == null)
                return NotFound();

            var updateReviewView = new UpdateReviewViewModel
            {
                Comment = review.Comment,
                ID = review.ID,
                UserID = review.UserID
            };

            return View(updateReviewView);
        }

        // POST: Handle the update submission of a review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReview(UpdateReviewViewModel updateReviewView)
        {
            if (ModelState.IsValid)
            {
                var done = await _reviewService.UpdateUserReviewAsync(updateReviewView.UserID, updateReviewView.ID, updateReviewView.Comment);
                if (done)
                {
                    return RedirectToAction("GetAllReviewsForCurrentUser");
                }

                ModelState.AddModelError("", "Failed to update review.");
            }
            return View(updateReviewView);
        }

        // GET: (Manager only) Display all reviews in the system
        [Authorize(Roles = "Manger")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            return View(reviews);
        }
    }
}
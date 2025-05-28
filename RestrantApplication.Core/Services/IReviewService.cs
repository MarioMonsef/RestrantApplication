using RestrantApplication.Core.Models.Review;
using RestrantApplication.Core.ViewModels.Review;

namespace RestrantApplication.Core.Services
{
    public interface IReviewService
    {
        /// <summary>
        /// Adds a new review from a user.
        /// </summary>
        /// <param name="UserID">The ID of the user adding the review.</param>
        /// <param name="Comment">The content of the review.</param>
        /// <returns>True if the review was added successfully; otherwise, false.</returns>
        Task<bool> AddUserReviewAsync(string UserID, string Comment);

        /// <summary>
        /// Deletes a review by its ID for a specific user.
        /// </summary>
        /// <param name="ReviewID">The ID of the review to delete.</param>
        /// <param name="userId">The ID of the user who owns the review.</param>
        /// <returns>True if the review was deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteUserReviewAsync(int ReviewID, string userId);

        /// <summary>
        /// Updates a user's review.
        /// </summary>
        /// <param name="UserID">The ID of the user updating the review.</param>
        /// <param name="ReviewID">The ID of the review to update.</param>
        /// <param name="Comment">The new content of the review.</param>
        /// <returns>True if the review was updated successfully; otherwise, false.</returns>
        Task<bool> UpdateUserReviewAsync(string UserID, int ReviewID, string Comment);

        /// <summary>
        /// Retrieves all reviews in the system.
        /// </summary>
        /// <returns>A read-only list of all reviews.</returns>
        Task<IReadOnlyList<ReviewViewModel>> GetReviewsAsync();

        /// <summary>
        /// Retrieves all reviews created by a specific user.
        /// </summary>
        /// <param name="UserID">The ID of the user whose reviews are to be retrieved.</param>
        /// <returns>A read-only list of the user's reviews.</returns>
        Task<IReadOnlyList<ReviewViewModel>> GetReviewsForUserAsync(string UserID);

        /// <summary>
        /// Gets a review by its ID.
        /// </summary>
        /// <param name="ReviewID">The ID of the review to retrieve.</param>
        /// <returns>The corresponding review if found; otherwise, null.</returns>
        Task<Review> GetReviewByIDAsync(int ReviewID);

        /// <summary>
        /// Deletes all cached reviews related to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose cached reviews will be deleted.</param>
        /// <returns>True if the cache was cleared successfully; otherwise, false.</returns>
        Task<bool> DeleteReviewsFromCacheAsync(string userId);
    }
}

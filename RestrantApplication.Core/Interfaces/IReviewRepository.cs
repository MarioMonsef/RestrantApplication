using RestrantApplication.Core.Models.Review;
using RestrantApplication.Core.ViewModels.Review;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Repository interface for review-related data operations.
    /// Extends the generic repository for basic CRUD operations.
    /// </summary>
    public interface IReviewRepository : IGenericRepository<Review>
    {
        /// <summary>
        /// Retrieves all reviews written by a specific user.
        /// </summary>
        /// <param name="UserID">The ID of the user whose reviews are to be retrieved.</param>
        /// <returns>A read-only list of review view models by the specified user.</returns>
        Task<IReadOnlyList<ReviewViewModel>> GetAllReviewsByUserIDAsync(string UserID);

        /// <summary>
        /// Retrieves all reviews along with related items data.
        /// </summary>
        /// <returns>A read-only list of review view models with associated items.</returns>
        Task<IReadOnlyList<ReviewViewModel>> GetAllReviewsAndItemsAsync();
    }
}

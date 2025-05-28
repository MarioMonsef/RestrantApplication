using Microsoft.IdentityModel.Tokens;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Review;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Review;

namespace RestrantApplication.EF.Services
{
    public class ReviewService : IReviewService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private const string HomeReviewsCacheKey = "Home_Reviews";

        #endregion

        #region Constructors

        public ReviewService(IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        #endregion

        #region Cache Helpers

        /// <summary>
        /// Generates a unique cache key for a specific user.
        /// </summary>
        private string GetReviewCacheKey(string userId) => $"Review_{userId}";

        /// <summary>
        /// Refreshes the cached review data for both the user and home page.
        /// </summary>
        private async Task RefreshReviewCacheAsync(string userId)
        {
            await _redisService.SetDataAsync(GetReviewCacheKey(userId), await _unitOfWork.ReviewRepository.GetAllReviewsByUserIDAsync(userId), TimeSpan.FromMinutes(30));
            await _redisService.SetDataAsync(HomeReviewsCacheKey, await _unitOfWork.ReviewRepository.GetAllReviewsAndItemsAsync(), TimeSpan.FromHours(1));
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Adds a review for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user adding the review.</param>
        /// <param name="Comment">The review comment.</param>
        /// <returns>True if added successfully, otherwise false.</returns>
        public async Task<bool> AddUserReviewAsync(string userId, string Comment)
        {
            if (userId.IsNullOrEmpty() || Comment.IsNullOrEmpty())
                return false;

            var review = new Review
            {
                Comment = Comment,
                UserID = userId
            };

            await _unitOfWork.ReviewRepository.AddAsync(review);
            await _unitOfWork.Complete();
            await RefreshReviewCacheAsync(userId);

            return true;
        }

        /// <summary>
        /// Deletes a user's review.
        /// </summary>
        /// <param name="ReviewID">The ID of the review to delete.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteUserReviewAsync(int ReviewID, string userId)
        {
            if (ReviewID <= 0)
                return false;

            await _unitOfWork.ReviewRepository.DeleteAsync(ReviewID);
            await _unitOfWork.Complete();
            await RefreshReviewCacheAsync(userId);

            return true;
        }

        /// <summary>
        /// Retrieves all reviews with item details (for home page).
        /// </summary>
        /// <returns>A Read Only list of reviews View Model.</returns>
        public async Task<IReadOnlyList<ReviewViewModel>> GetReviewsAsync()
        {
            var reviewsCache = await _redisService.GetDataAsync<IReadOnlyList<ReviewViewModel>>(HomeReviewsCacheKey);
            if (reviewsCache != null)
                return reviewsCache;

            var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsAndItemsAsync();
            await _redisService.SetDataAsync(HomeReviewsCacheKey, reviews, TimeSpan.FromHours(1));
            return reviews;
        }

        /// <summary>
        /// Retrieves all reviews for a specific user.
        /// </summary>
        /// <param name="UserID">The ID of the user.</param>
        /// <returns>A Read Only list of reviews View Model.</returns>
        public async Task<IReadOnlyList<ReviewViewModel>> GetReviewsForUserAsync(string UserID)
        {
            var cacheKey = GetReviewCacheKey(UserID);
            var reviewsInCache = await _redisService.GetDataAsync<IReadOnlyList<ReviewViewModel>>(cacheKey);
            if (reviewsInCache != null)
                return reviewsInCache;

            var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsByUserIDAsync(UserID);
            await _redisService.SetDataAsync(cacheKey, reviews, TimeSpan.FromMinutes(30));
            return reviews;
        }

        /// <summary>
        /// Updates a specific review for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="ReviewID">The ID of the review.</param>
        /// <param name="Comment">The new comment.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateUserReviewAsync(string userId, int ReviewID, string Comment)
        {
            if (ReviewID <= 0 || userId.IsNullOrEmpty())
                return false;

            var review = await _unitOfWork.ReviewRepository.GetByIDAsync(ReviewID);
            if (review == null)
                return false;

            review.Comment = Comment;
            _unitOfWork.ReviewRepository.Update(review);
            await _unitOfWork.Complete();
            await RefreshReviewCacheAsync(userId);

            return true;
        }

        /// <summary>
        /// Gets a review by its ID.
        /// </summary>
        /// <param name="ReviewID">The review ID.</param>
        /// <returns>The review object.</returns>
        public async Task<Review> GetReviewByIDAsync(int ReviewID) =>
            await _unitOfWork.ReviewRepository.GetByIDAsync(ReviewID);

        /// <summary>
        /// Deletes all reviews related to a user from the cache.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if cache key was removed.</returns>
        public async Task<bool> DeleteReviewsFromCacheAsync(string userId) =>
            await _redisService.DeleteDataAsync(GetReviewCacheKey(userId));

        #endregion
    }
}

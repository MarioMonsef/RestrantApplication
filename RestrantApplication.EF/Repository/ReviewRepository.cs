using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Review;
using RestrantApplication.Core.ViewModels;
using RestrantApplication.Core.ViewModels.Review;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    /// <summary>
    /// Repository class for handling review-related operations.
    /// Inherits from GenericRepository and implements IReviewRepository.
    /// </summary>
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        /// <param name="_context">The application's database context.</param>
        public ReviewRepository(AppDBContext _context) : base(_context)
        {
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="UserID">The ID of the user whose reviews are to be retrieved.</param>
        /// <returns>A read-only list of <see cref="ReviewViewModel"/>.</returns>
        public async Task<IReadOnlyList<ReviewViewModel>> GetAllReviewsByUserIDAsync(string UserID) =>
             await _context.Reviews
                .AsNoTracking()
                .Where(r => r.UserID == UserID)
                .Select(r => new ReviewViewModel
                {
                    ID = r.ID,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ApplictionUser = new ApplicationUserReviewViewModel
                    {
                        Id = r.ApplictionUser.Id,
                        Address = r.ApplictionUser.Address,
                        UserName = r.ApplictionUser.UserName
                    }
                })
                .OrderBy(r => r.ReviewDate)
                .ToListAsync();

        /// <summary>
        /// Retrieves all reviews along with user information and profile pictures.
        /// </summary>
        /// <returns>A read-only list of <see cref="ReviewViewModel"/> including user and picture details.</returns>
        public async Task<IReadOnlyList<ReviewViewModel>> GetAllReviewsAndItemsAsync() =>
             await _context.Reviews
                .AsNoTracking()
                .Select(r => new ReviewViewModel
                {
                    ID = r.ID,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ApplictionUser = new ApplicationUserReviewViewModel
                    {
                        Id = r.ApplictionUser.Id,
                        UserName = r.ApplictionUser.UserName,
                        Address = r.ApplictionUser.Address,
                        UserPicture = new UserPictureReviewViewModel
                        {
                            PictureName = r.ApplictionUser.UserPicture.PictureName
                        }
                    }
                })
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();

        #endregion
    }
}

using Microsoft.EntityFrameworkCore.Storage;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Represents a unit of work that manages repositories and database transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for managing categories.
        /// </summary>
        ICategoryRepository CategoryRepository { get; }

        /// <summary>
        /// Gets the repository for managing products.
        /// </summary>
        IProductRepository ProductRepository { get; }

        /// <summary>
        /// Gets the repository for managing photos.
        /// </summary>
        IPhotoRepository PhotoRepository { get; }

        /// <summary>
        /// Gets the repository for managing pictures.
        /// </summary>
        IPictureRepository PictureRepository { get; }

        /// <summary>
        /// Gets the repository for managing carts.
        /// </summary>
        ICartRepository CartRepository { get; }

        /// <summary>
        /// Gets the repository for managing orders.
        /// </summary>
        IOrderRepository OrderRepository { get; }

        /// <summary>
        /// Gets the repository for managing reviews.
        /// </summary>
        IReviewRepository ReviewRepository { get; }

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing the database transaction.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Saves all changes made in this unit of work to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the number of state entries written to the database.</returns>
        Task<int> Complete();
    }
}

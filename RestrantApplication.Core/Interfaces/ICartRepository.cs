using RestrantApplication.Core.Models.Cart;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Provides specific data access operations related to user carts.
    /// </summary>
    public interface ICartRepository : IGenericRepository<Cart>
    {
        /// <summary>
        /// Retrieves the cart for the specified user, or creates a new one if it doesn't exist.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user's cart.</returns>
        Task<Cart> GetOrCreateCartByUserIdAsync(string userId);
    }
}

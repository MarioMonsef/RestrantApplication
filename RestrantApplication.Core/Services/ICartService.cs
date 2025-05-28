using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.ViewModels.Cart;

namespace RestrantApplication.Core.Services
{
    public interface ICartService
    {
        /// <summary>
        /// Checks if the user has a cart. If not, creates a new one and returns it.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A CartViewModel representing the user's cart.</returns>
        Task<CartViewModel> CheckUserHasCartAndGetCartOrCreateAsync(string userId);

        /// <summary>
        /// Adds a product to the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="productId">The ID of the product to add.</param>
        /// <param name="quantity">The quantity of the product to add (default is 1).</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        Task<bool> AddItemToCartAsync(string userId, int productId, int quantity = 1);

        /// <summary>
        /// Removes a specific item from the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>True if the item was removed successfully, otherwise false.</returns>
        Task<bool> RemoveItemInCartAsync(string userId, int cartItemId);

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the cart was cleared successfully, otherwise false.</returns>
        Task<bool> ClearAllItemsInCartAsync(string userId);

        /// <summary>
        /// Deletes the user's cart from the cache.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the cart was removed from cache successfully, otherwise false.</returns>
        Task<bool> DeleteCartFromCacheAsync(string userId);
    }
}

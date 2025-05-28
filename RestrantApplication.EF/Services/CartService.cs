using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Cart;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Provides operations for managing the user's cart, including caching with Redis.
    /// </summary>
    public class CartService : ICartService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public CartService(IUnitOfWork unitOfWork, IRedisService redisService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _mapper = mapper;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Generates the Redis cache key for the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A string in the format "cart_{userId}".</returns>
        private string GetCartCacheKey(string userId) => $"cart_{userId}";

        /// <summary>
        /// Caches the given cart in Redis and returns the mapped view model.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="cart">The cart to be cached.</param>
        /// <returns>The <see cref="CartViewModel"/> representation of the cart.</returns>
        private async Task<CartViewModel> CacheCartAsync(string userId, Cart cart)
        {
            var cartVM = _mapper.Map<CartViewModel>(cart);
            await _redisService.SetDataAsync(GetCartCacheKey(userId), cartVM, TimeSpan.FromMinutes(30));
            return cartVM;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the user's cart from cache or database, or creates a new cart if none exists.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The <see cref="CartViewModel"/> if successful; otherwise, null.</returns>
        public async Task<CartViewModel> CheckUserHasCartAndGetCartOrCreateAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
                return default;

            var cacheKey = GetCartCacheKey(userId);

            // Try getting cart from Redis
            var cartCached = await _redisService.GetDataAsync<CartViewModel>(cacheKey);
            if (cartCached != null)
                return cartCached;

            // Load or create cart from DB then cache it
            var cart = await _unitOfWork.CartRepository.GetOrCreateCartByUserIdAsync(userId);
            return await CacheCartAsync(userId, cart);
        }

        /// <summary>
        /// Adds a product to the user's cart, or increases its quantity if already exists.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="productId">The product ID.</param>
        /// <param name="quantity">The quantity to add (default is 1).</param>
        /// <returns>True if the operation succeeded; otherwise, false.</returns>
        public async Task<bool> AddItemToCartAsync(string userId, int productId, int quantity = 1)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (userId.IsNullOrEmpty())
                    return false;

                // Check product existence and availability
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(productId, p => p.Photo);
                if (product == null || !product.IsAvilable)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var cart = await _unitOfWork.CartRepository.GetOrCreateCartByUserIdAsync(userId);

                // Check if product is already in the cart
                var existingItem = cart.CartItems.FirstOrDefault(c => c.ProductID == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        CartID = cart.ID,
                        ProductID = productId,
                        Quantity = quantity
                    });
                }

                var result = await _unitOfWork.Complete();
                if (result <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                await CacheCartAsync(userId, cart);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Decreases quantity of a specific item or removes it from the cart.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="cartItemId">The cart item ID to update.</param>
        /// <returns>True if item was updated or removed; otherwise, false.</returns>
        public async Task<bool> RemoveItemInCartAsync(string userId, int cartItemId)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (userId.IsNullOrEmpty())
                    return false;

                var cart = await _unitOfWork.CartRepository.GetOrCreateCartByUserIdAsync(userId);
                var existingItem = cart.CartItems.FirstOrDefault(c => c.ID == cartItemId);

                if (existingItem == null)
                {
                    await CacheCartAsync(userId, cart); 
                    return false;
                }

                // Reduce quantity or remove item
                if (existingItem.Quantity > 1)
                    existingItem.Quantity -= 1;
                else
                    cart.CartItems.Remove(existingItem);

                var result = await _unitOfWork.Complete();
                if (result <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                await CacheCartAsync(userId, cart);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Clears all items from the user's cart.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>True if all items were cleared; otherwise, false.</returns>
        public async Task<bool> ClearAllItemsInCartAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
                return false;
            try
            {
                var cart = await _unitOfWork.CartRepository.GetOrCreateCartByUserIdAsync(userId);
                cart.CartItems.Clear();

                var result = await _unitOfWork.Complete();
                if (result <= 0)
                    return false;

                await CacheCartAsync(userId, cart);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the user's cart from Redis cache.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>True if the cache was successfully deleted; otherwise, false.</returns>
        public async Task<bool> DeleteCartFromCacheAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
                return false;

            var cacheKey = GetCartCacheKey(userId);
            return await _redisService.DeleteDataAsync(cacheKey);
        }

        #endregion
    }
}

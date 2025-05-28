using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Cart;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    /// <summary>
    /// Repository class for Cart entity, inheriting from GenericRepository.
    /// Handles cart-specific data operations such as retrieval and creation.
    /// </summary>
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CartRepository"/> class using the specified database context.
        /// </summary>
        /// <param name="_context">The application database context.</param>
        public CartRepository(AppDBContext _context) : base(_context)
        {
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Retrieves an existing cart by user ID, or creates a new one if not found.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the cart.</param>
        /// <returns>The existing or newly created <see cref="Cart"/> object.</returns>
        public async Task<Cart> GetOrCreateCartByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return default;

            // Attempt to retrieve the user's cart along with its items, products, and product photos
            var cart = await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .ThenInclude(p => p.Photo)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            // If cart does not exist, create a new one for the user
            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId,
                    CartItems = new List<CartItem>()
                };

                await AddAsync(cart); // Add new cart to the database
                await _context.SaveChangesAsync(); // Persist changes
            }

            return cart;
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Services;
using System.Threading.Tasks;

namespace RestrantApplication.MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserContextService _userContextService;

        public CartController(ICartService cartService, IUserContextService userContextService)
        {
            _cartService = cartService;
            _userContextService = userContextService;
        }

        // Helper method to get current user ID from context service
        private string GetCurrentUserId() => _userContextService.GetCurrentUserId();

        // Display the current user's cart items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cart = await _cartService.CheckUserHasCartAndGetCartOrCreateAsync(userId);
            if (cart == null || cart.CartItems == null)
            {
                // If cart or items are null, pass empty list to view
                return View();
            }

            return View(cart.CartItems.ToList());
        }

        // Add a product to the user's cart
        public async Task<IActionResult> AddItemToCart(int productId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (productId <= 0)
            {
                TempData["Error"] = "Invalid product ID.";
                return RedirectToAction("Menu", "Product");
            }

            var isAdded = await _cartService.AddItemToCartAsync(userId, productId);
            if (isAdded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Failed to add the product to the cart.";
                return RedirectToAction("Menu", "Product");
            }
        }

        // Remove or decrease quantity of an item in the user's cart
        public async Task<IActionResult> RemoveItemInCart(int cartItemId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (cartItemId <= 0)
            {
                TempData["Error"] = "Invalid cart item ID.";
                return RedirectToAction("Index");
            }

            try
            {
                await _cartService.RemoveItemInCartAsync(userId, cartItemId);
            }
            catch
            {
                TempData["Error"] = "Failed to remove the item from the cart.";
            }

            return RedirectToAction("Index");
        }
    }
}

using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Order;
using RestrantApplication.EF.Services.Order__State_Design_Pattern_;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Service class responsible for managing orders, including creation,
    /// state changes, and retrieval of order data.
    /// </summary>
    public class OrderService : IOrderService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        #endregion

        #region Constructors

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }
        #endregion
        #region Handle Functions

        /// <summary>
        /// Creates a new order for the specified user based on their cart items.
        /// Uses a transaction to ensure atomicity.
        /// </summary>
        /// <param name="userId">The ID of the user placing the order.</param>
        /// <param name="orderType">The type of the order (e.g., delivery or pickup).</param>
        /// <returns>True if the order is successfully created; otherwise, false.</returns>
        public async Task<bool> CreateOrderAsync(string userId, OrderType orderType)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            // Start a database transaction to ensure all operations succeed or fail together
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Get the user's cart or create one if none exists
                var cart = await _cartService.CheckUserHasCartAndGetCartOrCreateAsync(userId);

                // If cart is null or empty, rollback and fail
                if (cart == null || !cart.CartItems.Any())
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Map cart items to order items with current price
                var orderItems = cart.CartItems.Select(c => new OrderItem
                {
                    Quantity = c.Quantity,
                    PriseAtOrder = c.Product.Prise,
                    ProductID = c.ProductID,
                }).ToList();

                // Create new order entity
                var newOrder = new Order
                {
                    UserID = userId,
                    OrderItems = orderItems,
                    TotalAmount = orderItems.Sum(c => c.Quantity * c.PriseAtOrder),
                    orderState = OrderState.Pending,
                    orderType = orderType
                };

                // Add order to repository
                await _unitOfWork.OrderRepository.AddAsync(newOrder);

                // Clear user's cart after order creation
                var isCleared = await _cartService.ClearAllItemsInCartAsync(userId);
                if (!isCleared)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Commit changes to database and transaction
                await _unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Rollback transaction on exception
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Changes the state of an existing order.
        /// Applies special rules for managers and delegates state change logic to handlers.
        /// </summary>
        /// <param name="orderID">The ID of the order to change.</param>
        /// <param name="userId">The ID of the user requesting the change (can be null).</param>
        /// <param name="newState">The new desired state of the order.</param>
        /// <param name="role">The role of the user requesting the change (e.g., Manager, Chef, Delivery).</param>
        /// <returns>True if the state change is successful; otherwise, false.</returns>
        public async Task<bool> ChangeOrderState(int orderID, string? userId, OrderState newState, string? role)
        {
            if (orderID <= 0)
                return false;

            var order = await _unitOfWork.OrderRepository.GetByIDAsync(orderID);
            if (order == null)
                return false;

            // If user is Manager, allow direct state change without restrictions
            if (role == "Manger")
            {
                order.orderState = newState;
                await _unitOfWork.Complete();
                return true;
            }

            // Get the appropriate handler for the current and new state
            var handler = OrderStateHandlerFactory.GetHandler(order, newState);
            if (handler == null)
                return false;

            // Perform the state change logic via the handler
            var success = handler.StateChange(order, userId, role);
            if (!success)
                return false;

            await _unitOfWork.Complete();
            return true;
        }

        /// <summary>
        /// Retrieves detailed information about an order by ID,
        /// including related entities (order items, products, etc.).
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order entity with related data or null if not found.</returns>
        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderWithRelatedDataAsync(orderId);
            return order;
        }

        /// <summary>
        /// Retrieves a list of orders filtered optionally by user ID and role flags.
        /// Supports filtering orders for chef and delivery roles.
        /// </summary>
        /// <param name="userID">The user ID to filter orders by (optional).</param>
        /// <param name="isChef">Filter orders relevant for chef (default: false).</param>
        /// <param name="isDelivary">Filter orders relevant for delivery (default: false).</param>
        /// <returns>A read-only list of order view models matching the criteria.</returns>
        public async Task<IReadOnlyList<OrderViewModel>> GetAllOrdersAsync(string? userID, bool isChef = false, bool isDelivary = false)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersAsync(userID, isChef, isDelivary);
            return orders.ToList();
        }
        #endregion
    }
}

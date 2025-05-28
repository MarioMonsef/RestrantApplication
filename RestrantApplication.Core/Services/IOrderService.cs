using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.ViewModels.Order;

namespace RestrantApplication.Core.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user placing the order.</param>
        /// <param name="orderType">The type of the order (e.g., dine-in, delivery).</param>
        /// <returns>True if the order was created successfully; otherwise, false.</returns>
        Task<bool> CreateOrderAsync(string userId, OrderType orderType);

        /// <summary>
        /// Changes the state of an existing order.
        /// </summary>
        /// <param name="orderID">The ID of the order to update.</param>
        /// <param name="userId">The ID of the user requesting the change.</param>
        /// <param name="orderState">The new state of the order (e.g., Preparing, Delivered).</param>
        /// <param name="Role">The role of the user making the change (e.g., Admin, Chef, Delivery).</param>
        /// <returns>True if the state was changed successfully; otherwise, false.</returns>
        Task<bool> ChangeOrderState(int orderID, string userId, OrderState orderState, string? Role);

        /// <summary>
        /// Retrieves detailed information about a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order object with detailed information if found; otherwise, null.</returns>
        Task<Order> GetOrderDetailsAsync(int orderId);

        /// <summary>
        /// Retrieves all orders based on user role and filters.
        /// </summary>
        /// <param name="userID">Optional user ID to filter orders by user.</param>
        /// <param name="isChef">Whether to filter for chef-related orders.</param>
        /// <param name="isDelivary">Whether to filter for delivery-related orders.</param>
        /// <returns>A list of orders matching the criteria.</returns>
        Task<IReadOnlyList<OrderViewModel>> GetAllOrdersAsync(string? userID, bool isChef = false, bool isDelivary = false);
    }
}

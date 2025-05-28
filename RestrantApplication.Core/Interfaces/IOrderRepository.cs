using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.ViewModels.Order;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Provides data access operations specific to orders.
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>
        /// Retrieves a read-only list of orders filtered by user and role criteria.
        /// </summary>
        /// <param name="userId">Optional user ID to filter orders.</param>
        /// <param name="isChef">Flag indicating if the request is from a chef role.</param>
        /// <param name="isDelivary">Flag indicating if the request is from a delivery role.</param>
        /// <returns>A read-only list of orders matching the criteria.</returns>
        Task<IReadOnlyList<OrderViewModel>> GetOrdersAsync(string? userId, bool isChef = false, bool isDelivary = false);

        /// <summary>
        /// Retrieves a specific order along with its related data (e.g., items, user info).
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order entity with related data.</returns>
        Task<Order> GetOrderWithRelatedDataAsync(int orderId);
    }
}

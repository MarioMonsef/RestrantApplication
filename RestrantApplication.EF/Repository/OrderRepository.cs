using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.ViewModels.Order;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    /// <summary>
    /// Repository for managing Order entities with additional
    /// methods to retrieve orders with related data and filtered lists.
    /// Extends the generic repository.
    /// </summary>
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        #region Fields
        private readonly IMapper _mapper;
        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="_context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance for projection.</param>
        public OrderRepository(AppDBContext _context, IMapper mapper) : base(_context)
        {
            _mapper = mapper;
        }

        #endregion

        #region Handel Functions

        /// <summary>
        /// Retrieves an order by ID including its related entities:
        /// order items, their products, and product photos.
        /// Uses AsNoTracking for read-only optimization.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order entity with related data or null if not found.</returns>
        public async Task<Order> GetOrderWithRelatedDataAsync(int orderId)
        {
            var order = await _context.Order
                    .AsNoTracking()
                    .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Photo)
                    .FirstOrDefaultAsync(predicate: o => o.ID == orderId);

            return order;
        }

        /// <summary>
        /// Retrieves a filtered list of orders projected to OrderViewModel.
        /// Can filter by userId, and role-based filters for chef or delivery.
        /// Limits orders by date range depending on whether userId is provided.
        /// </summary>
        /// <param name="userId">Optional user ID to filter orders for a specific user.</param>
        /// <param name="isChef">Flag indicating to filter orders relevant to chef role.</param>
        /// <param name="isDelivary">Flag indicating to filter orders relevant to delivery role.</param>
        /// <returns>A read-only list of order view models.</returns>
        public async Task<IReadOnlyList<OrderViewModel>> GetOrdersAsync(string? userId, bool isChef = false, bool isDelivary = false)
        {
            var query = _context.Order
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .Include(o => o.ApplictionUser)  // Include user info for display
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                // Filter by user ID
                query = query.Where(o => o.UserID == userId);

                // Filter orders within last and next month
                query = query.Where(o => o.OrderDate <= DateTime.Now.AddMonths(1) && o.OrderDate >= DateTime.Now.AddMonths(-1));
            }
            else
            {
                // For general queries without user, filter to orders within +/- 1 day only
                query = query.Where(o => o.OrderDate <= DateTime.Now.AddDays(1) && o.OrderDate >= DateTime.Now.AddDays(-1));
            }

            if (isChef)
            {
                // Filter orders that are pending or processing for chef role
                query = query.Where(o => o.orderState == OrderState.Pending || o.orderState == OrderState.Processing);
            }

            if (isDelivary)
            {
                // Filter orders that are in delivery or completed state and are delivery type
                query = query.Where(o =>
                    (o.orderState == OrderState.DeliveryNow || o.orderState == OrderState.Completed) &&
                    o.orderType == OrderType.Delivery);
            }
            query = query.OrderByDescending(o => o.OrderDate);

            // Project query result to OrderViewModel using AutoMapper
            return await query
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        #endregion
    }
}

using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Shared;
using RestrantApplication.Core.ViewModels.Product;

namespace RestrantApplication.Core.Interfaces
{
    /// <summary>
    /// Repository interface for product data operations.
    /// Extends the generic repository for basic CRUD operations.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Retrieves a read-only list of products filtered and paginated by the provided parameters.
        /// </summary>
        /// <param name="productParams">Parameters to filter, sort, and paginate products.</param>
        /// <returns>A read-only list of product view models.</returns>
        Task<IReadOnlyList<ProductViewModel>> GetAllAsync(ProductParams productParams);
    }
}

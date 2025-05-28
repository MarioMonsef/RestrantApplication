using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Shared;
using RestrantApplication.Core.ViewModels.Product;

namespace RestrantApplication.Core.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a product by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product entity if found; otherwise, null.</returns>
        Task<Product> GetProductByIDAsync(int id);

        /// <summary>
        /// Adds a new product with its image to the website.
        /// </summary>
        /// <param name="AddProductAndImageViewModel">The view model containing product and image data.</param>
        /// <returns>True if the product was added successfully; otherwise, false.</returns>
        Task<bool> AddProductToWebSite(AddProductAndImageViewModel AddProductAndImageViewModel);

        /// <summary>
        /// Retrieves a list of all products based on provided filtering and paging parameters.
        /// </summary>
        /// <param name="productParams">Filtering and paging parameters.</param>
        /// <returns>A read-only list of product view models.</returns>
        Task<IReadOnlyList<ProductViewModel>> GetAllProductAsync(ProductParams productParams);

        /// <summary>
        /// Gets the total number of products.
        /// </summary>
        /// <returns>The number of products.</returns>
        Task<int> Count();

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="ID">The ID of the product to delete.</param>
        /// <returns>True if the product was deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteProductAsync(int ID);

        /// <summary>
        /// Updates a product's information on the website.
        /// </summary>
        /// <param name="updateProduct">The updated product data.</param>
        /// <returns>True if the product was updated successfully; otherwise, false.</returns>
        Task<bool> UpdateProductToWebSite(UpdateProductViewModel updateProduct);
    }
}

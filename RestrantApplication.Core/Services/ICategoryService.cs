using RestrantApplication.Core.Models.Product;

namespace RestrantApplication.Core.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Adds a new category to the website asynchronously.
        /// </summary>
        /// <param name="category">The category object to be added.</param>
        /// <returns>True if the category was added successfully; otherwise, false.</returns>
        Task<bool> AddCategoryToWebsiteAsync(Category category);

        /// <summary>
        /// Retrieves all categories from the website asynchronously.
        /// </summary>
        /// <returns>A read-only list of all categories.</returns>
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category by its ID asynchronously.
        /// </summary>
        /// <param name="ID">The ID of the category to retrieve.</param>
        /// <returns>The category object if found; otherwise, null.</returns>
        Task<Category> GetCategoryByIDAsync(int ID);

        /// <summary>
        /// Updates an existing category asynchronously.
        /// </summary>
        /// <param name="category">The category object with updated data.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Deletes a category by its ID asynchronously.
        /// </summary>
        /// <param name="CategoryID">The ID of the category to delete.</param>
        /// <returns>True if the category was deleted successfully; otherwise, false.</returns>
        Task<bool> DeleteCategoryByIDAsync(int CategoryID);
    }
}

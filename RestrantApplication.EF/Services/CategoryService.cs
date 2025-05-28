using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Services;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Service responsible for managing product categories (CRUD operations).
    /// </summary>
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Adds a new category to the website.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>True if the category is added successfully; otherwise, false.</returns>
        public async Task<bool> AddCategoryToWebsiteAsync(Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return false;

            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.Complete();
            return true;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A read-only list of all categories.</returns>
        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync() =>
            await _unitOfWork.CategoryRepository.GetAllAsync();

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category if found; otherwise, null.</returns>
        public async Task<Category> GetCategoryByIDAsync(int id) =>
            await _unitOfWork.CategoryRepository.GetByIDAsync(id);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The updated category object.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            if (category == null || category.ID <= 0)
                return false;

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var categoryOld = await _unitOfWork.CategoryRepository.GetByIDAsync(category.ID);
                if (categoryOld == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                categoryOld.Name = category.Name;
                categoryOld.Description = category.Description;

                _unitOfWork.CategoryRepository.Update(categoryOld);
                await _unitOfWork.Complete();
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
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        /// <returns>True if the category was deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteCategoryByIDAsync(int categoryId)
        {
            if (categoryId <= 0)
                return false;

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(categoryId);
                await _unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        #endregion
    }
}

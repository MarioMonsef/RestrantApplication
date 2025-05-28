using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Shared;
using RestrantApplication.Core.ViewModels.Cart;
using RestrantApplication.Core.ViewModels.Product;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    /// <summary>
    /// Repository for managing product-related data operations.
    /// Inherits from GenericRepository and implements IProductRepository.
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMapper _mapper;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="_context">The application's database context.</param>
        /// <param name="mapper">The AutoMapper instance used for object mapping.</param>
        public ProductRepository(AppDBContext _context, IMapper mapper) : base(_context)
        {
            _mapper = mapper;
        }

        #endregion

        #region Handle Functions

        /// <summary>
        /// Retrieves a filtered and paginated list of products based on the specified parameters.
        /// </summary>
        /// <param name="productParams">Filtering and pagination parameters.</param>
        /// <returns>A read-only list of matching products as <see cref="ProductViewModel"/>.</returns>
        public async Task<IReadOnlyList<ProductViewModel>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(p => p.Photo)
                .Include(p => p.category)
                .AsNoTracking();

            // Apply keyword search if specified
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                // Filter where every search word matches either name or description
                query = query.Where(p => searchWords.All(word =>
                    p.Name.ToLower().Contains(word.ToLower()) ||
                    p.Description.ToLower().Contains(word.ToLower())
                ));
            }

            // Filter by category if specified
            if (productParams.CategoryID.HasValue)
            {
                query = query.Where(p => p.categoryID == productParams.CategoryID.Value);
            }

            // Ensure pagination numbers are valid (fallback to defaults if invalid)
            productParams.PageNumber = productParams.PageNumber > 0 ? productParams.PageNumber : 1;
            productParams.PageSize = productParams.PageSize > 0 ? productParams.PageSize : 9;

            // Apply pagination to query
            query = query
                .Skip(productParams.PageSize * (productParams.PageNumber - 1))
                .Take(productParams.PageSize);

            // Project to view model and execute query
            return await query
                .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        #endregion
    }
}

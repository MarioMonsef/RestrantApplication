using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private IPhotoRepository _photoRepository;
        private IPictureRepository _pictureRepository;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
        private IReviewRepository _reviewRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the UnitOfWork class with injected database context.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public UnitOfWork(AppDBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the category repository for managing category data.
        /// </summary>
        public ICategoryRepository CategoryRepository =>
            _categoryRepository ??= new CategoryRepository(_context);

        /// <summary>
        /// Gets the product repository for managing product data.
        /// </summary>
        public IProductRepository ProductRepository =>
            _productRepository ??= new ProductRepository(_context,_mapper);

        /// <summary>
        /// Gets the photo repository for managing photo data.
        /// </summary>
        public IPhotoRepository PhotoRepository =>
            _photoRepository ??= new PhotoRepository(_context);

        /// <summary>
        /// Gets the picture repository for managing picture data.
        /// </summary>
        public IPictureRepository PictureRepository =>
            _pictureRepository ??= new PictureRepository(_context);

        /// <summary>
        /// Gets the cart repository for managing shopping cart data.
        /// </summary>
        public ICartRepository CartRepository =>
            _cartRepository ??= new CartRepository(_context);

        /// <summary>
        /// Gets the order repository for managing order data.
        /// </summary>
        public IOrderRepository OrderRepository =>
            _orderRepository ??= new OrderRepository(_context,_mapper);

        /// <summary>
        /// Gets the review repository for managing review data.
        /// </summary>
        public IReviewRepository ReviewRepository =>
            _reviewRepository ??= new ReviewRepository(_context);

        #endregion

        #region Methods

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the database context and frees resources.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation containing the database transaction.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        #endregion
    }
}

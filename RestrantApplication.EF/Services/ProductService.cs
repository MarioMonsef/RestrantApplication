using AutoMapper;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.Shared;
using RestrantApplication.Core.ViewModels.Cart;
using RestrantApplication.Core.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// Service to handle product-related operations, including CRUD and caching with Redis.
    /// </summary>
    public class ProductService : IProductService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;

        private const string ProductCachePattern = "Products_Key_*";

        #endregion

        #region Constructors


        public ProductService(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
            _redisService = redisService;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates a normalized Redis cache key based on product parameters.
        /// </summary>
        /// <param name="parameters">The product search and paging parameters.</param>
        /// <returns>A string key for caching products.</returns>
        private string GetProductCacheKey(ProductParams parameters)
        {
            var normalizedSearch = parameters.Search?.Trim().ToLower() ?? "null";
            var categoryId = parameters.CategoryID ?? 0;

            return $"Products_Key_Category{categoryId}_Search{normalizedSearch}_Page{parameters.PageNumber}_Size{parameters.PageSize}";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates an existing product on the website, including its image if provided.
        /// </summary>
        /// <param name="updateProduct">The updated product data including image.</param>
        /// <returns>True if the update is successful; otherwise, false.</returns>
        public async Task<bool> UpdateProductToWebSite(UpdateProductViewModel updateProduct)
        {
            if (updateProduct == null)
                return false;

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (updateProduct.Image != null)
                {
                    var updatedPhoto = await _photoService.UpdatePhotoAsync(updateProduct.OldPhotoID, updateProduct.Image);
                    if (updatedPhoto == null)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                var product = _mapper.Map<Product>(updateProduct);
                _unitOfWork.ProductRepository.Update(product);

                await _unitOfWork.Complete();

                // Remove cache keys related to products to ensure data consistency
                await _redisService.RemoveByPatternAsync(ProductCachePattern);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Adds a new product along with its image to the website.
        /// </summary>
        /// <param name="addProductAndImageViewModel">The new product and image data.</param>
        /// <returns>True if the addition is successful; otherwise, false.</returns>
        public async Task<bool> AddProductToWebSite(AddProductAndImageViewModel addProductAndImageViewModel)
        {
            if (addProductAndImageViewModel == null)
                return false;

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                Photo photo = await _photoService.UploadImage(addProductAndImageViewModel.Image);
                if (photo == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var product = _mapper.Map<Product>(addProductAndImageViewModel);
                product.PhotoID = photo.ID;

                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.Complete();

                // Remove all product cache keys
                await _redisService.RemoveByPatternAsync(ProductCachePattern);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Retrieves a product by its ID, including its photo.
        /// </summary>
        /// <param name="ID">The product ID.</param>
        /// <returns>The product entity if found; otherwise, null.</returns>
        public async Task<Product> GetProductByIDAsync(int ID)
        {
            return await _unitOfWork.ProductRepository.GetByIDAsync(ID, c => c.Photo);
        }

        /// <summary>
        /// Retrieves all products filtered and paged according to parameters, using Redis caching.
        /// </summary>
        /// <param name="productParams">The search and paging parameters.</param>
        /// <returns>A read-only list of products.</returns>
        public async Task<IReadOnlyList<ProductViewModel>> GetAllProductAsync(ProductParams productParams)
        {
            var cacheKey = GetProductCacheKey(productParams);

            // Try get data from Redis cache first
            var productsCache = await _redisService.GetDataAsync<IReadOnlyList<ProductViewModel>>(cacheKey);
            if (productsCache != null)
                return productsCache;

            // If not cached, retrieve from DB
            var products = await _unitOfWork.ProductRepository.GetAllAsync(productParams);

            // Cache the retrieved products for 6 hours
            await _redisService.SetDataAsync(cacheKey, products, TimeSpan.FromHours(6));

            return products;
        }

        /// <summary>
        /// Counts the total number of products.
        /// </summary>
        /// <returns>The count of products in the database.</returns>
        public async Task<int> Count() =>
            await _unitOfWork.ProductRepository.Count();

        /// <summary>
        /// Deletes a product and its associated photo, including cache invalidation.
        /// </summary>
        /// <param name="ID">The product ID to delete.</param>
        /// <returns>True if deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteProductAsync(int ID)
        {
            if (ID <= 0) return false;

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIDAsync(ID, c => c.Photo);
                if (product == null)
                    return false;

                await _unitOfWork.ProductRepository.DeleteAsync(product.ID);

                if (product.Photo != null)
                {
                    var deleteSuccess = await _photoService.DeleteImageByID(product.Photo.ID);
                    if (!deleteSuccess)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                await _unitOfWork.Complete();

                // Invalidate product caches
                await _redisService.RemoveByPatternAsync(ProductCachePattern);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        #endregion
    }
}

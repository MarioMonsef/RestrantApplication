// Controller responsible for handling all product-related operations (Add, Update, Delete, List, Search)
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Product;
using RestrantApplication.MVC.Helper;
namespace RestrantApplication.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public ProductController(IProductService productService, ICategoryService categoryService, IMapper mapper,IPhotoService photoService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
            _photoService = photoService;
        }

        // Helper method to load all categories into the ViewBag
        private async Task SetCategoriesToViewBag()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        }

        // Display the Add Product form
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            await SetCategoriesToViewBag();
            return View();
        }

        // Handle the submission of the Add Product form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductAndImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // If model is invalid, reload categories and redisplay form
                await SetCategoriesToViewBag();
                return View(model);
            }

            // Try to add the product via the service
            var result = await _productService.AddProductToWebSite(model);
            if (result)
            {
                return RedirectToAction("Menu");
            }

            // If addition fails, show an error and return the form
            ModelState.AddModelError("", "Failed to add product. Please try again.");
            await SetCategoriesToViewBag();
            return View(model);
        }

        // Display the Update Product form
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int ID)
        {
            var product = await _productService.GetProductByIDAsync(ID);
            if (product == null)
                return NotFound();

            await SetCategoriesToViewBag();

            // Map the product entity to Update ViewModel
            var updateProductVm = _mapper.Map<UpdateProductViewModel>(product);
            return View(updateProductVm);
        }

        // Handle the submission of the Update Product form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoriesToViewBag();
                return View(model);
            }

            var result = await _productService.UpdateProductToWebSite(model);
            if (result)
            {
                return RedirectToAction("Menu");
            }

            ModelState.AddModelError("", "Failed to update product. Please try again.");
            await SetCategoriesToViewBag();
            return View(model);
        }

        // Display the product list with pagination and support for AJAX partial updates
        public async Task<IActionResult> Menu(int pageNumber = 1, int pageSize = 9)
        {
            var productParams = new RestrantApplication.Core.Shared.ProductParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var products = await _productService.GetAllProductAsync(productParams);
            var totalCount = await _productService.Count();

            var pagedList = new Pagination<ProductViewModel>(pageNumber, pageSize, totalCount, products);

            // Check if the request is an AJAX call (used for loading partial views)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_MenuuPartialView", pagedList.Data);
            }

            await SetCategoriesToViewBag();
            return View(pagedList);
        }

        // Perform product search with pagination and AJAX support
        public async Task<IActionResult> Search(string searchTerm, int pageNumber = 1)
        {
            const int pageSize = 9;

            var productParams = new RestrantApplication.Core.Shared.ProductParams
            {
                Search = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var products = await _productService.GetAllProductAsync(productParams);
            var totalCount = await _productService.Count();

            // Handle AJAX partial view update for real-time search results
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductGridPartial", products);
            }

            var model = new Pagination<ProductViewModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = products
            };

            return View(model);
        }

        // Delete a product - restricted to users with the "Manager" role
        [Authorize(Roles = "Manger")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {   
            // Call the service to delete the product
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                TempData["Error"] = "Failed to delete the product or product not found.";
            }
            else
            {
                TempData["Success"] = "Product deleted successfully.";
            }

            return RedirectToAction("Menu");
        }
    }
}
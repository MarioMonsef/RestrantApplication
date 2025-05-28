using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.Shared;
using RestrantApplication.Core.ViewModels;

namespace RestrantApplication.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;

        public HomeController(IProductService productService, ICategoryService categoryService, IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
        }

        // Main home page action
        public async Task<IActionResult> Home()
        {
            var productParams = new ProductParams(); 

            var products = await _productService.GetAllProductAsync(productParams);
            var totalCount = await _productService.Count();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            var reviews = await _reviewService.GetReviewsAsync();

            var homeData = new HomeDataViewModel
            {
                TotalCount = totalCount,
                Products = products,
                PageSize = productParams.PageSize,
                PageNumber = productParams.PageNumber,
                Reviews = reviews,
            };

            return View(homeData);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}

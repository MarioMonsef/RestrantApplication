using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Services;
using System.Threading.Tasks;

namespace RestrantApplication.MVC.Controllers
{
    [Authorize(Roles = "Manger")] // Protect all actions by default
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Show the form for adding a new category
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        // Handle the submission of the new category form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var isAdded = await _categoryService.AddCategoryToWebsiteAsync(category);

            if (isAdded)
                return RedirectToAction("Menu", "Product");

            ModelState.AddModelError("", "Failed to add category. Please try again.");
            return View(category);
        }

        // Load category data by ID to update it
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int categoryId)
        {
            var category = await _categoryService.GetCategoryByIDAsync(categoryId);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // Handle the submission of the updated category form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var isUpdated = await _categoryService.UpdateCategoryAsync(category);

            if (isUpdated)
                return RedirectToAction("GetAllCategory");

            ModelState.AddModelError("", "Failed to update category. Please try again.");
            return View(category);
        }

        // Display a list of all categories
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // Handle deletion of category by ID
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var isDeleted = await _categoryService.DeleteCategoryByIDAsync(categoryId);

            if (isDeleted)
                return RedirectToAction("GetAllCategory");

            TempData["ErrorMessage"] = "Can't delete category. Please try again.";
            return RedirectToAction("GetAllCategory");
        }
    }
}

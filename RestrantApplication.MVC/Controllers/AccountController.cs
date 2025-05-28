using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.Services;
using RestrantApplication.Core.ViewModels.Identity;

namespace RestrantApplication.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPictureService _pictureService;
        private readonly IUserContextService _userContextService;
        private readonly ICartService _cartService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPictureService pictureService, IUserContextService userContextService, ICartService cartService, IReviewService reviewService, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _pictureService = pictureService;
            _userContextService = userContextService;
            _cartService = cartService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // Show registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle user registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
                return View(register);

            UserPicture picture = await _pictureService.UploadPictureAsync(register.Picture);
            if (picture != null)
            {
                var applicationUser = _mapper.Map<ApplicationUser>(register);
                applicationUser.PictureID = picture.ID;

                IdentityResult result = await _userManager.CreateAsync(applicationUser, register.Password);
                if (result.Succeeded)
                {
                    IdentityResult result1 = await _userManager.AddToRoleAsync(applicationUser, "Client");
                    if (result1.Succeeded)
                    {
                        await _signInManager.SignInAsync(applicationUser, false);
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        // Add role errors to ModelState
                        foreach (var error in result1.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(register);
                    }
                }
                else
                {
                    // Add creation errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(register);
                }
            }
            else
            {
                ModelState.AddModelError("", "Failed to upload picture.");
                return View(register);
            }
            
        }

        // Show login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handle user login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (applicationUser != null)
                {
                    var found = await _userManager.CheckPasswordAsync(applicationUser, loginViewModel.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(applicationUser, loginViewModel.RemeberMe);
                        return RedirectToAction("Home", "Home");
                    }
                }
                // General error for failed login
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginViewModel);
        }

        // Logout user and clear cached data
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            var userId = _userContextService.GetCurrentUserId();
            if (userId != null)
            {
                await _cartService.DeleteCartFromCacheAsync(userId);
                await _reviewService.DeleteReviewsFromCacheAsync(userId);
            }

            return RedirectToAction("Home", "Home");
        }

        // Show current user's information
        [Authorize]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = _userContextService.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null) return NotFound();

            ViewBag.PictureName = await _pictureService.GetPictureNameAsync(applicationUser.PictureID);
            return View(applicationUser);
        }

        // Show form to update user address
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateAddress()
        {
            var userId = _userContextService.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null) return NotFound();

            var updateAddress = new UpdateAddressViewModel
            {
                Address = applicationUser.Address,
            };

            return View(updateAddress);
        }

        // Handle update of user address
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(UpdateAddressViewModel updateAddress)
        {
            if (!ModelState.IsValid)
                return View(updateAddress);

            var userId = _userContextService.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null) return NotFound();

            applicationUser.Address = updateAddress.Address;
            var result = await _userManager.UpdateAsync(applicationUser);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmOrder", controllerName: "Order", new { orderType = updateAddress.orderType });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(updateAddress);
            }
            
        }

        // Show registration page for admin to create users
        [Authorize(Roles ="Manger")]
        [HttpGet]
        public IActionResult RegisterByAdmin()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        // Handle admin user registration
        [Authorize(Roles = "Manger")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterByAdmin(RegisterByAdminViewModel registerByAdmin)
        {
            ViewBag.Roles = _roleManager.Roles.ToList();

            if (!ModelState.IsValid)
                return View(registerByAdmin);

            var picture = await _pictureService.UploadPictureAsync(registerByAdmin.Picture);
            if (picture == null)
            {
                ModelState.AddModelError("", "Failed to upload picture.");
                return View(registerByAdmin);
            }

            var user = _mapper.Map<ApplicationUser>(registerByAdmin);
            user.PictureID = picture.ID;

            var result = await _userManager.CreateAsync(user, registerByAdmin.PasswordHash);
            if (result.Succeeded)
            {
                var resultR = await _userManager.AddToRoleAsync(user, registerByAdmin.Role);
                if (resultR.Succeeded)
                {
                    TempData["SuccessMessage"] = "User registered successfully.";
                    return RedirectToAction("RegisterByAdmin");
                }
                else
                {
                    foreach (var error in resultR.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(registerByAdmin);
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerByAdmin);
            }
        }

        // Delete current user account
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = _userContextService.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(actionName: "GetUserInformation");
            }
            if (user.PictureID != null)
            {
                var isPictureDeleted = await _pictureService.DeletePictureInDiskAsync(user.PictureID);
                if (!isPictureDeleted)
                {
                    TempData["Error"] = "Failed to delete profile picture.";
                }
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Home", "Home");
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction("GetUserInformation");
            }

        }
    }
}

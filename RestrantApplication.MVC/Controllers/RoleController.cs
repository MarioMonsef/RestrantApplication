using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.ViewModels.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestrantApplication.MVC.Controllers
{
    [Authorize(Roles = "Manger")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: Display all roles in the system
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: Show the form to add a new role
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        // POST: Handle new role creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleViewModel addRole)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole { Name = addRole.RoleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Role added successfully.";
                    return RedirectToAction("GetAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(addRole);
        }

        // GET: Delete a role by ID
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    TempData[key: "Error"] = "Not Found this Role.";
                    return RedirectToAction("GetAllRoles");
                }
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role deleted successfully.";
                    return RedirectToAction("GetAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    TempData[key: "Error"] = error.Description;
                }
                return RedirectToAction("GetAllRoles");
            }
            TempData["Error"] = "Invalid role ID.";
            return RedirectToAction("GetAllRoles");
        }

        // GET: Show the update form for a specific role
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var roleViewModel = new UpdateRoleViewModel
                    {
                        RoleName = role.Name,
                        ID = role.Id
                    };
                    return View(roleViewModel);
                }
                TempData[key: "Error"] = "Not Found this Role.";
            }
            TempData["Error"] = "Invalid role ID.";
            return RedirectToAction("GetAllRoles");
        }

        // POST: Handle role update submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(roleViewModel.ID);
                if (role == null)
                    return NotFound();

                role.Name = roleViewModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role updated successfully.";
                    return RedirectToAction("GetAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(roleViewModel);
        }
    }
}

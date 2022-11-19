using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AirsoftMatchMaker.Core.Models.RoleRequests;

namespace AirsoftMatchMaker.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class VendorsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IVendorService vendorService;

        public VendorsController(UserManager<User> userManager, IVendorService vendorService, IRoleRequestService roleRequestService)
        {
            this.userManager = userManager;
            this.vendorService = vendorService;
        }
        public async Task<IActionResult> GrantVendorRole(string userId, int roleRequestId)
        {
            await vendorService.GrantVendorRoleAsync(userId);
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Vendor",
                RoleRequestId = roleRequestId.ToString()
            };
            return RedirectToAction("GrantRole", "RoleRequests", model);
        }

        public async Task<IActionResult> RemoveFromVendorRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData.Add("error", "There is no such user");
                return RedirectToAction("Index", "RoleRequests");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Vendor"))
            {
                TempData.Add("error", "User is not a vendor!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await vendorService.RemoveFromVendorRoleAsync(user.Id);
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Vendor"
            };
            return RedirectToAction("RemoveFromRole", "RoleRequests", model);
        }

        public async Task<IActionResult> Index()
        {
            var model = await vendorService.GetAllVendorsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await vendorService.GetVendorByIdAsync(id);
            return View(model);
        }

    }
}

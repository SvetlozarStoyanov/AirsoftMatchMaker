using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private readonly IVendorService vendorService;

        public VendorsController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }

        public IActionResult RequestVendorRole()
        {
            if (User.IsInRole("Player") || User.IsInRole("Matchmaker") || User.IsInRole("Vendor"))
            {
                TempData.Add("error", "You cannot be assigned to that role!");
                return RedirectToAction("Index", "RoleRequests");
            }
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Vendor"
            };
            return RedirectToAction("RequestRole", "RoleRequests", model);
        }

        public async Task<IActionResult> LeaveVendorRole()
        {
            if (!User.IsInRole("Vendor"))
            {
                TempData.Add("error", "You are not a vendor!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await vendorService.RemoveFromVendorRoleAsync(User.Id());

            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Vendor"
            };

            return RedirectToAction("LeaveRole", "RoleRequests", model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await vendorService.GetAllVendorsAsync();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var model = await vendorService.GetVendorByIdAsync(id);
            return View(model);
        }

    }
}

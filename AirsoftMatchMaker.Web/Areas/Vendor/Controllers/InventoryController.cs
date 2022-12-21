using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Inventory;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class InventoryController : Controller
    {

        private readonly IInventoryService inventoryService;
        private readonly IUserService userService;
        private readonly IVendorService vendorService;
        public InventoryController(IInventoryService inventoryService, IUserService userService, IVendorService vendorService)
        {
            this.inventoryService = inventoryService;
            this.userService = userService;
            this.vendorService = vendorService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await inventoryService.GetVendorItemsAsync(User.Id());
            ViewBag.UserCredits = await userService.GetUserCreditsAsync(User.Id());
            ViewBag.VendorId = await vendorService.GetVendorIdAsync(User.Id());
            return View(model);
        }
        public async Task<IActionResult> PlayerItems()
        {
            var model = await inventoryService.GetPlayerItemsAsync(User.Id());
            return View(model);
        }
    }
}

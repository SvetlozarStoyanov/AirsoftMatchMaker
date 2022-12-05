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
        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await inventoryService.GetVendorItemsAsync(User.Id());
            return View(model);
        }
        public async Task<IActionResult> PlayerItems()
        {
            var model = await inventoryService.GetPlayerItemsAsync(User.Id());
            return View(model);
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Inventory;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class InventoryController : Controller
    {

        private readonly IInventoryService inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await inventoryService.GetPlayerItemsAsync(User.Id());

            return View(model);
        }


    }
}

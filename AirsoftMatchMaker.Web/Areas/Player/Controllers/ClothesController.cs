using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class ClothesController : Controller
    {
        private readonly IClothingService clothingService;
        public ClothesController(IClothingService clothingService)
        {
            this.clothingService = clothingService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await clothingService.GetAllClothesAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await clothingService.ClothingExistsAsync(id)))
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await clothingService.GetClothingByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            if (!(await clothingService.ClothingExistsAsync(id)))
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await clothingService.UserCanBuyClothingAsync(User.Id(), id)))
            {
                TempData["error"] = $"You cannot buy the clothing you imported!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await clothingService.UserHasEnoughCreditsAsync(User.Id(), id)))
            {
                TempData["error"] = $"You do not have enough credits to buy this item!";
                return RedirectToAction(nameof(Index));
            }
            var model = await clothingService.GetClothingByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int clothingId, int vendorId)
        {
            await clothingService.BuyClothingAsync(User.Id(), vendorId, clothingId);
            return RedirectToAction(nameof(Index));
        }
    }
}

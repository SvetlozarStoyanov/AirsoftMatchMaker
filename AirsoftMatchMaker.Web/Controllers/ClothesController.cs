using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
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
            var model = await clothingService.GetClothingByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            var model = await clothingService.GetClothingByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int clothingId, int vendorId)
        {
            await clothingService.BuyClothingAsync(User.Id(),vendorId, clothingId);
            return RedirectToAction(nameof(Index));
        }
    }
}

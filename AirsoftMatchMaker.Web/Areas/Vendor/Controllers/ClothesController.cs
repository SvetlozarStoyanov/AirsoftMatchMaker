using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ClothesController : Controller
    {
        private readonly IClothingService clothingService;
        private readonly IVendorService vendorService;

        public ClothesController(IClothingService clothingService, IVendorService vendorService)
        {
            this.clothingService = clothingService;
            this.vendorService = vendorService;
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
        public async Task<IActionResult> Sell(int id)
        {
            if (!(await clothingService.ClothingExistsAsync(id)))
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction("PlayerItems", "Inventory");
            }
            if (!(await clothingService.UserCanSellClothingAsync(User.Id(), id)))
            {
                TempData["error"] = $"You cannot sell that item!";
                return RedirectToAction("PlayerItems", "Inventory");
            }
            var model = await clothingService.CreateClothingSellModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(ClothingSellModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await clothingService.CreateClothingSellModelAsync(model.Id);
                return View(model);
            }
            await clothingService.SellClothingAsync(User.Id(), model);
            TempData["success"] = $"{model.Name} is now for sale!";
            return RedirectToAction("PlayerItems", "Inventory");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ClothingCreateModel()
            {
                Colors = Enum.GetValues<ClothingColor>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClothingCreateModel model)
        {
            if (!ModelState.IsValid || !await vendorService.CheckIfVendorHasEnoughCreditsAsync(User.Id(), model.FinalImportPrice))
            {
                model.Colors = Enum.GetValues<ClothingColor>();
                return View(model);
            }
            await clothingService.CreateClothingAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

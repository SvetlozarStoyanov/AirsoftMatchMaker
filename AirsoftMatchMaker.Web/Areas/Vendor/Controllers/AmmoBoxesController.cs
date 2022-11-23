using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class AmmoBoxesController : Controller
    {
        private readonly IAmmoBoxService ammoBoxService;
        private readonly IVendorService vendorService;

        public AmmoBoxesController(IAmmoBoxService ammoBoxService, IVendorService vendorService)
        {
            this.ammoBoxService = ammoBoxService;
            this.vendorService = vendorService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await ammoBoxService.GetAllAmmoBoxesAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await ammoBoxService.GetAmmoBoxByIdAsync(id);
            if (model != null)
            {
                return View(model);
            }
            TempData.Add("error", $"Ammo box with {id} id does not exist!");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            var model = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Buy(AmmoBoxBuyModel model)
        {
            await ammoBoxService.BuyAmmoBoxAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AmmoBoxCreateModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmmoBoxCreateModel model)
        {
            if (!ModelState.IsValid || !await vendorService.CheckIfVendorHasEnoughCreditsAsync(User.Id(), model.FinalImportPrice))
            {
                return View(model);
            }
          
            await ammoBoxService.CreateAmmoBoxAsync(User.Id(),model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Restock(int id)
        {
            var model = await ammoBoxService.GetAmmoBoxForRestockByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Restock(AmmoBoxRestockModel model)
        {
            if (!ModelState.IsValid || !await vendorService.CheckIfVendorHasEnoughCreditsAsync(User.Id(), model.FinalImportPrice))
            {
                return View(model);
            }
            await ammoBoxService.RestockAmmoBox(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

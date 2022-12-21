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
        private readonly IHtmlSanitizingService htmlSanitizingService;
        private readonly IUserService userService;

        public AmmoBoxesController(IAmmoBoxService ammoBoxService, IVendorService vendorService, IHtmlSanitizingService htmlSanitizingService, IUserService userService)
        {
            this.ammoBoxService = ammoBoxService;
            this.vendorService = vendorService;
            this.htmlSanitizingService = htmlSanitizingService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index([FromQuery] AmmoBoxesQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await ammoBoxService.GetAllAmmoBoxesAsync(
                model.SearchTerm,
                model.Sorting,
                model.AmmoBoxesPerPage,
                model.CurrentPage
                );
            model.AmmoBoxes = queryResult.AmmoBoxes;
            model.AmmoBoxesCount = queryResult.AmmoBoxesCount;
            model.SortingOptions = queryResult.SortingOptions;

            ViewBag.VendorId = await vendorService.GetVendorIdAsync(User.Id());
            ViewBag.UserCredits = await userService.GetUserCreditsAsync(User.Id());

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(id)))
            {
                TempData.Add("error", $"Ammo box does not exist!");

                return RedirectToAction(nameof(Index));
            }
            var model = await ammoBoxService.GetAmmoBoxByIdAsync(id);
            ViewBag.VendorId = await vendorService.GetVendorIdAsync(User.Id());
            ViewBag.UserCredits = await userService.GetUserCreditsAsync(User.Id());

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AmmoBoxCreateModel();
            ViewBag.UserCredits = await userService.GetUserCreditsAsync(User.Id());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmmoBoxCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model = htmlSanitizingService.SanitizeObject<AmmoBoxCreateModel>(model);
            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await vendorService.CheckIfVendorHasEnoughCreditsAsync(User.Id(), model.FinalImportPrice))
            {
                TempData["error"] = "You do no have enough credits to import this item!";
                return View(model);
            }

            await ammoBoxService.CreateAmmoBoxAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Restock(int id)
        {
            var model = await ammoBoxService.GetAmmoBoxForRestockByIdAsync(id);
            ViewBag.VendorId = await vendorService.GetVendorIdAsync(User.Id());
            if (model.VendorId != ViewBag.VendorId)
            {
                TempData["error"] = "You cannot restock this item!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserCredits = await userService.GetUserCreditsAsync(User.Id());
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

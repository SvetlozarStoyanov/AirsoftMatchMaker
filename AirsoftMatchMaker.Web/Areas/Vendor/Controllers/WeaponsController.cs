using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class WeaponsController : Controller
    {
        private readonly IWeaponService weaponService;
        private readonly IVendorService vendorService;

        public WeaponsController(IWeaponService weaponService, IVendorService vendorService)
        {
            this.weaponService = weaponService;
            this.vendorService = vendorService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await weaponService.GetAllWeaponsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await weaponService.GetWeaponByIdAsync(id);
            if (model == null)
            {
                TempData.Add("error", "There is no weapon with that id!");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Sell(int id)
        {
            if (!(await weaponService.WeaponExistsAsync(id)))
            {
                TempData["error"] = $"Weapon with {id} does not exist!";
                return RedirectToAction("PlayerItems", "Inventory");
            }
            if (!(await weaponService.UserCanSellWeaponAsync(User.Id(), id)))
            {
                TempData["error"] = $"You cannot sell that weapon!";
                return RedirectToAction("PlayerItems", "Inventory");
            }
            var model = await weaponService.CreateWeaponSellModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(WeaponSellModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await weaponService.CreateWeaponSellModelAsync(model.Id);
                return View(model);
            }
            await weaponService.SellWeaponAsync(User.Id(), model);
            return RedirectToAction("PlayerItems", "Inventory");

        }

        [HttpGet]
        public IActionResult SelectWeaponType()
        {
            WeaponCreateTypeSelectModel selectModel = new WeaponCreateTypeSelectModel()
            {
                WeaponTypes = Enum.GetValues<WeaponType>()
            };
            return View(selectModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(WeaponCreateTypeSelectModel selectModel)
        {
            if (!ModelState.IsValid)
            {
                return View(selectModel);
            }
            var createModel = weaponService.CreateWeaponCreateModelByWeaponType(selectModel.WeaponType);
            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(WeaponCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model = weaponService.CreateWeaponCreateModelByWeaponType(model.WeaponType);
                return View(model);
            }
            if (!(await vendorService.CheckIfVendorHasEnoughCreditsAsync(User.Id(), model.FinalImportPrice)))
            {
                TempData["error"] = "You do not have enough credits to import this weapon!";
                model = weaponService.CreateWeaponCreateModelByWeaponType(model.WeaponType);
                return View(model);
            }
            await weaponService.CreateWeaponAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

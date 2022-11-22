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

        public WeaponsController(IWeaponService weaponService)
        {
            this.weaponService = weaponService;
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
        public async Task<IActionResult> Buy(int id)
        {
            var model = await weaponService.GetWeaponByIdAsync(id);
            if (model == null)
            {
                TempData.Add("error", "There is no weapon with that id!");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int weaponid, int vendorId)
        {
            await weaponService.BuyWeaponAsync(User.Id(), vendorId, weaponid);
            return RedirectToAction(nameof(Index));
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
                return View("Create", model);
            }
            var errors = weaponService.ValidateWeaponParameters(model);
            if (errors.Count() > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View("Create", model);
            }
            await weaponService.CreateWeaponAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

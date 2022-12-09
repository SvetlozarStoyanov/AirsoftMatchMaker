using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class WeaponsController : Controller
    {
        private readonly IWeaponService weaponService;
        private readonly IHtmlSanitizingService htmlSanitizingService;

        public WeaponsController(IWeaponService weaponService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.weaponService = weaponService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index([FromQuery] WeaponsQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await weaponService.GetAllWeaponsAsync(
                                 model.WeaponType,
                                 model.PreferedEngagementDistance,
                                 model.Sorting,
                                 model.SearchTerm,
                                 model.WeaponsPerPage,
                                 model.CurrentPage);
            model.WeaponTypes = queryResult.WeaponTypes;
            model.PreferedEngagementDistances = queryResult.PreferedEngagementDistances;
            model.SortingOptions = queryResult.SortingOptions;
            model.WeaponsCount = queryResult.WeaponsCount;
            model.Weapons = queryResult.Weapons;
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
            if (!(await weaponService.UserCanBuyWeaponAsync(User.Id(), id)))
            {
                TempData["error"] = "You cannot buy weapons you imported!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await weaponService.UserHasEnoughCreditsAsync(User.Id(), id)))
            {
                TempData["error"] = "You do not have enough credits!";
                return RedirectToAction(nameof(Index));
            }
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
    }
}

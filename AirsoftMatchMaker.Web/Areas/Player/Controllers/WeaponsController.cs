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
        private readonly IPlayerService playerService;

        public WeaponsController(IWeaponService weaponService, IHtmlSanitizingService htmlSanitizingService, IPlayerService playerService)
        {
            this.weaponService = weaponService;
            this.htmlSanitizingService = htmlSanitizingService;
            this.playerService = playerService;
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
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await weaponService.WeaponExistsAsync(id)))
            {
                TempData.Add("error", "Weapon does not exist");
                return RedirectToAction(nameof(Index));
            }
            var model = await weaponService.GetWeaponByIdAsync(id);
            ViewBag.BuyWeaponListModel = await weaponService.GetWeaponListModelForBuyAsync(id);
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Buy(int id)
        {
            
            if (!(await weaponService.WeaponExistsAsync(id)))
            {
                TempData.Add("error", "Weapon does not exist");
                return RedirectToAction(nameof(Index));
            }
            if (!(await weaponService.WeaponIsForSaleAsync(id)))
            {
                TempData.Add("error", "Weapon is not for sale!");
                return RedirectToAction(nameof(Index));
            }
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
            await weaponService.BuyWeaponAsync(User.Id(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}

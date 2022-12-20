using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class AmmoBoxesController : Controller
    {
        private readonly IAmmoBoxService ammoBoxService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        private readonly IPlayerService playerService;
        public AmmoBoxesController(IAmmoBoxService ammoBoxService, IHtmlSanitizingService htmlSanitizingService, IPlayerService playerService)
        {
            this.ammoBoxService = ammoBoxService;
            this.htmlSanitizingService = htmlSanitizingService;
            this.playerService = playerService;
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
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(id)))
            {
                TempData["error"] = $"Ammo box  does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await ammoBoxService.GetAmmoBoxByIdAsync(id);
            ViewBag.BuyAmmoBoxModel = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(id);
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(id)))
            {
                TempData["error"] = $"Ammo box  does not exist!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await ammoBoxService.UserCanBuyAmmoBoxAsync(User.Id(), id)))
            {
                TempData["error"] = $"You cannot buy ammo boxes you imported!";
                return RedirectToAction(nameof(Index));
            }
            var model = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(id);
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(AmmoBoxBuyModel model)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(model.Id)))
            {
                TempData["error"] = $"Ammo box  does not exist!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await ammoBoxService.UserCanBuyAmmoBoxAsync(User.Id(), model.Id)))
            {
                TempData["error"] = $"You cannot buy ammo boxes you imported!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await ammoBoxService.UserHasEnoughCreditsAsync(User.Id(), model.Id, model.QuantityToBuy)))
            {
                TempData["error"] = $"You do not have enough credits!";
                model = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(model.Id);
                return View(model);
            }
            await ammoBoxService.BuyAmmoBoxAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

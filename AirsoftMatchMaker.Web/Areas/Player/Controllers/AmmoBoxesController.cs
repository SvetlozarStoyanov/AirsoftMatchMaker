using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
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
        public AmmoBoxesController(IAmmoBoxService ammoBoxService)
        {
            this.ammoBoxService = ammoBoxService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await ammoBoxService.GetAllAmmoBoxesAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(id)))
            {
                TempData["error"] = $"Ammo box with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await ammoBoxService.GetAmmoBoxByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id)
        {
            if (!(await ammoBoxService.AmmoBoxExistsAsync(id)))
            {
                TempData["error"] = $"Ammo box with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            if (!(await ammoBoxService.UserCanBuyAmmoBoxAsync(User.Id(), id)))
            {
                TempData["error"] = $"You cannot buy ammo boxes you imported!";
                return RedirectToAction(nameof(Index));
            }
            var model = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(AmmoBoxBuyModel model)
        {
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

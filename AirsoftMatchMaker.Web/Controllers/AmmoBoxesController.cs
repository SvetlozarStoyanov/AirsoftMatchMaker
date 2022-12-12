using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class AmmoBoxesController : Controller
    {
        private readonly IAmmoBoxService ammoBoxService;
        private readonly IHtmlSanitizingService htmlSanitizingService;

        public AmmoBoxesController(IAmmoBoxService ammoBoxService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.ammoBoxService = ammoBoxService;
            this.htmlSanitizingService = htmlSanitizingService;
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
        [Authorize(Roles = "Player")]
        public async Task<IActionResult> Buy(AmmoBoxBuyModel model)
        {
            await ammoBoxService.BuyAmmoBoxAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

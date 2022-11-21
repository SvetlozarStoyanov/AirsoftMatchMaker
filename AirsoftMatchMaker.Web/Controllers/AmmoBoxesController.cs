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

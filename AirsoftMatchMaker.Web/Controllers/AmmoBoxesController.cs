using AirsoftMatchMaker.Core.Contracts;
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
    }
}

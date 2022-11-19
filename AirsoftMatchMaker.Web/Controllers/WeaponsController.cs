using AirsoftMatchMaker.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
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
    }
}

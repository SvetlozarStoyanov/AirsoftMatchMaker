using AirsoftMatchMaker.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class MapsController : Controller
    {
        private readonly IMapService mapService;

        public MapsController(IMapService mapService)
        {
            this.mapService = mapService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await mapService.GetAllMapsAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await mapService.GetMapByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Map with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

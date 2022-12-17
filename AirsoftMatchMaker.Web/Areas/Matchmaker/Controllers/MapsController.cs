using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Matchmaker.Controllers
{
    [Area("Matchmaker")]
    [Authorize(Roles = "Matchmaker")]
    public class MapsController : Controller
    {
        private readonly IMapService mapService;
        private readonly IHtmlSanitizingService htmlSanitizingService;

        public MapsController(IMapService mapService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.mapService = mapService;
            this.htmlSanitizingService = htmlSanitizingService;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await mapService.CreateMapCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MapCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await mapService.CreateMapCreateModelAsync();
                return View(model);
            }
            if (await mapService.MapAlreadyExists(model.Name))
            {
                TempData["error"] = $"Map with {model.Name} already exists!";
                model = await mapService.CreateMapCreateModelAsync();
                return View(model);
            }
            model = htmlSanitizingService.SanitizeObject<MapCreateModel>(model);
            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                model = await mapService.CreateMapCreateModelAsync();
                return View(model);
            }
            if (await mapService.MapAlreadyExists(model.Name))
            {
                TempData["error"] = $"Map with name {model.Name} already exists!";
                model = await mapService.CreateMapCreateModelAsync();
                return View(model);
            }
            await mapService.CreateMapAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}

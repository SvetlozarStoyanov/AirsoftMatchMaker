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

        public async Task<IActionResult> Index([FromQuery] MapsQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            model.GameModeName = htmlSanitizingService.SanitizeStringProperty(model.GameModeName);
            var queryResult = await mapService.GetAllMapsAsync(
                model.SearchTerm,
                model.GameModeName,
                model.Sorting,
                model.MapsPerPage,
                model.CurrentPage
                );
            model.Maps = queryResult.Maps;
            model.MapsCount = queryResult.MapsCount;
            model.SortingOptions = queryResult.SortingOptions;
            model.GameModeNames = queryResult.GameModeNames;
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await mapService.MapExistsAsync(id)))
            {
                TempData["error"] = "Map does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await mapService.GetMapByIdAsync(id);
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
            if (await mapService.IsMapNameAlreadyTaken(model.Name))
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
            if (await mapService.IsMapNameAlreadyTaken(model.Name))
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

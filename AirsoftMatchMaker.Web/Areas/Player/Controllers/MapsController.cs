using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
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

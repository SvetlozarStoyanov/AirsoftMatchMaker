using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class GameModesController : Controller
    {
        private readonly IGameModeService gameModeService;
        private readonly IHtmlSanitizingService htmlSanitizingService;

        public GameModesController(IGameModeService gameModeService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.gameModeService = gameModeService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index([FromQuery] GameModesQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await gameModeService.GetAllGameModesAsync(
                model.SearchTerm,
                model.Sorting,
                model.GameModesPerPage,
                model.CurrentPage
                );
            model.GameModes = queryResult.GameModes;
            model.SortingOptions = queryResult.SortingOptions;
            model.GameModesCount = queryResult.GameModesCount;
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await gameModeService.GetGameModeByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Game mode with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

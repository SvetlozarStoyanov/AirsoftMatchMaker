using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IMapService mapService;
        private readonly IGameModeService gameModeService;

        public GamesController(IGameService gameService, IMapService mapService, IGameModeService gameModeService)
        {
            this.gameService = gameService;
            this.mapService = mapService;
            this.gameModeService = gameModeService;
        }

        public async Task<IActionResult> Index([FromQuery] GamesQueryModel model)
        {
            var queryResult = await gameService.GetAllGamesAsync(
                model.TeamName,
                model.GameModeName,
                model.GameStatus,
                model.Sorting,
                model.GamesPerPage,
                model.CurrentPage
                );
            model.Games = queryResult.Games;
            model.GamesCount = queryResult.GamesCount;
            model.GameStatuses = queryResult.GameStatuses;
            model.GameModeNames = queryResult.GameModeNames;
            model.TeamNames = queryResult.TeamNames;
            model.SortingOptions = queryResult.SortingOptions;
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (!(await gameService.GameExistsAsync(id)))
            {
                TempData["error"] = "Game does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await gameService.GetGameByIdAsync(id);
            ViewBag.Map = await mapService.GetMapByIdAsync(model.Map.Id);
            ViewBag.GameMode = await gameModeService.GetGameModeByIdAsync(model.Map.GameModeId);
            return View(model);
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IGameModeService gameModeService;
        private readonly IMapService mapService;
        private readonly IBetService betService;
        public GamesController(IGameService gameService, IGameModeService gameModeService, IMapService mapService, IBetService betService)
        {
            this.gameService = gameService;
            this.gameModeService = gameModeService;
            this.mapService = mapService;
            this.betService = betService;
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
            ViewBag.GameIdsOfGamesUserHasBetOn = await betService.GetGamesIdsWhichUserHasBetOnAsync(User.Id());
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
            ViewBag.GameIdsOfGamesUserHasBetOn = await betService.GetGamesIdsWhichUserHasBetOnAsync(User.Id());
            ViewBag.Map = await mapService.GetMapByIdAsync(model.Map.Id);
            ViewBag.GameMode = await gameModeService.GetGameModeByIdAsync(model.Map.GameModeId);
            return View(model);
        }
    }
}

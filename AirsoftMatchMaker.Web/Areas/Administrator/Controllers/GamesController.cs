using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IGameSimulationService gameSimulationService;
        private readonly IBetService betService;
        private readonly IMapService mapService;
        private readonly IGameModeService gameModeService;
        public GamesController(IGameService gameService, IGameSimulationService gameSimulationService, IBetService betService, IMapService mapService, IGameModeService gameModeService)
        {
            this.gameService = gameService;
            this.gameSimulationService = gameSimulationService;
            this.betService = betService;
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
                TempData["error"] = $"Game with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await gameService.GetGameByIdAsync(id);
            ViewBag.Map = await mapService.GetMapByIdAsync(model.Map.Id);
            ViewBag.GameMode = await gameModeService.GetGameModeByIdAsync(model.Map.GameModeId);
            if (await gameService.GameIsFinishedButNotFinalisedAsync(id))
            {
                ViewBag.FinaliseGameModel = await gameService.CreateGameFinaliseModelAsync(id);
            }
            return View(model);
        }

        public async Task<IActionResult> Simulate(int id)
        {
            await gameSimulationService.SimulateGameAsync(id);
            await gameSimulationService.PayoutBetsByGameIdAsync(id);
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Finalise(int id)
        {
            if (!(await gameService.GameExistsAsync(id)))
            {
                TempData["error"] = "Game does not exist!";
                return RedirectToAction(nameof(Index));

            }
            if (!(await gameService.GameIsFinishedButNotFinalisedAsync(id)))
            {
                TempData["error"] = "Game is not finished!";
                return RedirectToAction(nameof(Index));
            }
            var model = await gameService.CreateGameFinaliseModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Finalise(GameFinaliseModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await gameService.FinalizeGameAsync(model);
            await betService.PayoutBetsByGameIdAsync(model.Id);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
    }
}

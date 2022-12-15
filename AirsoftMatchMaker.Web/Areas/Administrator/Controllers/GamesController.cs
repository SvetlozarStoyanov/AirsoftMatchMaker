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
        public GamesController(IGameService gameService, IGameSimulationService gameSimulationService)
        {
            this.gameService = gameService;
            this.gameSimulationService = gameSimulationService;
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
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
    }
}

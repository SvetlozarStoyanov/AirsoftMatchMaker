using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Matchmaker.Controllers
{
    [Area("Matchmaker")]
    [Authorize(Roles = "Matchmaker")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IBetService betService;
        private readonly IMatchmakerService matchmakerService;
        private readonly IMapService mapService;
        private readonly IGameModeService gameModeService;

        public GamesController(IGameService gameService, IBetService betService, IMatchmakerService matchmakerService, IMapService mapService, IGameModeService gameModeService)
        {
            this.gameService = gameService;
            this.betService = betService;
            this.matchmakerService = matchmakerService;
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
                TempData["error"] = $"Game  does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await gameService.GetGameByIdAsync(id);
            ViewBag.Map = await mapService.GetMapByIdAsync(model.Map.Id);
            ViewBag.GameMode = await gameModeService.GetGameModeByIdAsync(model.Map.GameModeId);
            if (await gameService.GameCanBeFinalisedAsync(User.Id(), id) && await gameService.GameIsFinishedButNotFinalisedAsync(id))
            {
                ViewBag.FinaliseGameModel = await gameService.CreateGameFinaliseModelAsync(id);
            }
            return View(model);
        }

        public async Task<IActionResult> SelectGameDate()
        {
            var selectDateModel = await gameService.GetNextSevenAvailableDatesAsync();
            if (selectDateModel == null)
            {
                TempData["error"] = "No games can be made right now!";
                return RedirectToAction(nameof(Index));
            }
            return View(selectDateModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(GameSelectDateModel selectDateModel)
        {
            var createModel = await gameService.CreateGameCreateModelAsync(selectDateModel.DateTime);
            return View(createModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateModel model)
        {
            if (!ModelState.IsValid || model.TeamRedId == model.TeamBlueId)
            {
                model = await gameService.CreateGameCreateModelAsync(model.DateString);
                return View(model);
            }
            if (!(await gameService.AreTeamPlayerCountsSimilarAsync(model.TeamRedId, model.TeamBlueId)))
            {
                TempData["error"] = "Player count difference cannot be greater than 2 players!";
                model = await gameService.CreateGameCreateModelAsync(model.DateString);
                return View(model);
            }
            await gameService.CreateGameAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Mine([FromQuery] GamesMatchmakerQueryModel model)
        {
            if (model.MatchmakerId == 0)
            {
                model.MatchmakerId = await matchmakerService.GetMatchmakerIdAsync(User.Id());
            }
            var queryResult = await gameService.GetAllGamesForAdminAndMatchmakerAsync(
                model.MatchmakerId,
                model.MatchmakerGameStatus,
                model.Sorting,
                model.GamesPerPage,
                model.CurrentPage);

            model.Games = queryResult.Games;
            model.GamesCount = queryResult.GamesCount;
            model.MatchmakerGameStatuses = queryResult.MatchmakerGameStatuses;
            model.SortingOptions = queryResult.SortingOptions;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Finalise(int id)
        {
            if (!(await gameService.GameExistsAsync(id)))
            {
                TempData["error"] = "Game does not exist!";
                return RedirectToAction(nameof(Mine));

            }
            if (!(await gameService.GameCanBeFinalisedAsync(User.Id(), id)))
            {
                TempData["error"] = "Game can only by finalised by its creator!";
                return RedirectToAction(nameof(Mine));
            }
            if (!(await gameService.GameIsFinishedButNotFinalisedAsync(id)))
            {
                TempData["error"] = "Game is not finished!";
                return RedirectToAction(nameof(Mine));
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

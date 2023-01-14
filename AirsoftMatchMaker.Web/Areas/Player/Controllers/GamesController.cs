using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IMapService mapService;
        private readonly IGameModeService gameModeService;
        private readonly IPlayerService playerService;
        private readonly IBetService betService;

        public GamesController(IGameService gameService, IMapService mapService, IGameModeService gameModeService, IPlayerService playerService, IBetService betService)
        {
            this.gameService = gameService;
            this.mapService = mapService;
            this.gameModeService = gameModeService;
            this.playerService = playerService;
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
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            ViewBag.UserTeamId = await playerService.GetPlayersTeamIdAsync(User.Id());
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
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            ViewBag.UserTeamId = await playerService.GetPlayersTeamIdAsync(User.Id());
            return View(model);
        }
    }
}

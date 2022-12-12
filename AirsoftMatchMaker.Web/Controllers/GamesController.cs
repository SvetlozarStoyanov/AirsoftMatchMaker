using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService gameService;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
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
            var model = await gameService.GetGameByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Game with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

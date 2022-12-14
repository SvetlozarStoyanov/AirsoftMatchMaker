using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
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
            if (!(await gameService.GameExistsAsync(id)))
            {
                TempData["error"] = $"Game with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await gameService.GetGameByIdAsync(id);

            if (await gameService.GameCanBeFinalisedByMatchmakerAsync(User.Id(), id) && await gameService.GameIsFinishedButNotFinalisedAsync(id))
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

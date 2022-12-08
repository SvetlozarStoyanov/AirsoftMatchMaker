using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

        public async Task<IActionResult> Index()
        {
            var model = await gameService.GetAllGamesAsync();
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
            var createModel = await gameService.CreateGameModelAsync(selectDateModel.DateTime);
            return View(createModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateModel model)
        {
            if (!ModelState.IsValid || model.TeamRedId == model.TeamBlueId)
            {
                model = await gameService.CreateGameModelAsync(model.DateString);
                return View(model);
            }
            if (!(await gameService.AreTeamPlayerCountsSimilarAsync(model.TeamRedId, model.TeamBlueId)))
            {
                TempData["error"] = "Player count difference cannot be greater than 2 players!";
                model = await gameService.CreateGameModelAsync(model.DateString);
                return View(model);
            }
            await gameService.CreateGameAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

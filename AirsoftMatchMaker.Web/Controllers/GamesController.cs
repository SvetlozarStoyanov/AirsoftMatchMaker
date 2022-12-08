using AirsoftMatchMaker.Core.Contracts;
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

        public async Task<IActionResult> Index()
        {
            var model = await gameService.GetAllGamesAsync();
            ViewBag.FinishedGames = model.Where(g => g.GameStatus == GameStatus.Finished);
            ViewBag.UpcomingGames = model.Where(g => g.GameStatus == GameStatus.Upcoming);
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

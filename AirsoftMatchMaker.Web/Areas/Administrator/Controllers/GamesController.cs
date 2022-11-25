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

        public async Task<IActionResult> Simulate(int id)
        {
            await gameService.SimulateGameAsync(id);
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}

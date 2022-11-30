using AirsoftMatchMaker.Core.Contracts;
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
            await gameSimulationService.SimulateGameAsync(id);
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}

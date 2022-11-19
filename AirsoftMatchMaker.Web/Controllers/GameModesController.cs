using AirsoftMatchMaker.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class GameModesController : Controller
    {
        private readonly IGameModeService gameModeService;
        public GameModesController(IGameModeService gameModeService)
        {
            this.gameModeService = gameModeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await gameModeService.GetAllGameModesAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await gameModeService.GetGameModeByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Game mode with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;
        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await teamService.GetAllTeamsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await teamService.GetTeamByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Team with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}

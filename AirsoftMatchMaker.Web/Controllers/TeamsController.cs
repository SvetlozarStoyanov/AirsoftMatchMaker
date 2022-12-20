using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Teams;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        public TeamsController(ITeamService teamService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.teamService = teamService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index([FromQuery]TeamsQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await teamService.GetAllTeamsAsync(
                model.SearchTerm,
                model.Sorting,
                model.TeamsPerPage,
                model.CurrentPage
                );
            model.SortingOptions = queryResult.SortingOptions;
            model.TeamsCount = queryResult.TeamsCount;
            model.Teams = queryResult.Teams;
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

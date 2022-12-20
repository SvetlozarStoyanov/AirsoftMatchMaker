using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        public TeamsController(ITeamService teamService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.teamService = teamService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index([FromQuery] TeamsQueryModel model)
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
            if (!(await teamService.TeamExistsAsync(id)))
            {
                TempData["error"] = $"Team does not exist!";
                return RedirectToAction(nameof(Index));
            }
            var model = await teamService.GetTeamByIdAsync(id);
            return View(model);
        }
    }
}

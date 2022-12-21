using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles ="Vendor")]
    public class PlayersController : Controller
    {
        private readonly IPlayerService playerService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        private readonly ITeamService teamService;
        public PlayersController(IPlayerService playerService, IHtmlSanitizingService htmlSanitizingService, ITeamService teamService)
        {
            this.playerService = playerService;
            this.htmlSanitizingService = htmlSanitizingService;
            this.teamService = teamService;
        }

        public IActionResult RequestPlayerRole()
        {
            if (User.IsInRole("Player") || User.IsInRole("Vendor") || User.IsInRole("Matchmaker"))
            {
                TempData.Add("error", "You cannot be assigned to that role!");
                return RedirectToAction("Index", "RoleRequests");
            }
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Player"
            };
            return RedirectToAction("RequestRole", "RoleRequests", model);
        }

        public async Task<IActionResult> LeavePlayerRole()
        {
            if (!User.IsInRole("Player"))
            {
                TempData.Add("error", "You are not a player!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await playerService.RemoveFromPlayerRoleAsync(User.Id());
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Player"
            };
            return RedirectToAction("LeaveRole", "RoleRequests", model);
        }

        public async Task<IActionResult> Index([FromQuery] PlayersQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await playerService.GetAllPlayersAsync(
                model.SearchTerm,
                model.Sorting,
                model.PlayersPerPage,
                model.CurrentPage);
            model.PlayersCount = queryResult.PlayersCount;
            model.Players = queryResult.Players;
            model.SortingOptions = queryResult.SortingOptions;
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await playerService.PlayerExistsAsync(id)))
            {
                TempData.Add("error", $"Player does not exist!");
                return RedirectToAction(nameof(Index));
            }
            var model = await playerService.GetPlayerByIdAsync(id);
            var teamId = await playerService.GetPlayersTeamIdAsync(id);
            if (teamId.HasValue)
                ViewBag.PlayerTeam = await teamService.GetTeamByIdAsync(teamId.Value);
            else
                ViewBag.PlayerTeam = null;
            return View(model);
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class PlayersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IPlayerService playerService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        private readonly ITeamService teamService;

        public PlayersController(UserManager<User> userManager, IPlayerService playerService, IHtmlSanitizingService htmlSanitizingService, ITeamService teamService)
        {
            this.userManager = userManager;
            this.playerService = playerService;
            this.htmlSanitizingService = htmlSanitizingService;
            this.teamService = teamService;
        }
        public async Task<IActionResult> GrantPlayerRole(string userId, int roleRequestId)
        {
            await playerService.GrantPlayerRoleAsync(userId);
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Player",
                RoleRequestId = roleRequestId.ToString()
            };
            return RedirectToAction("GrantRole", "RoleRequests", model);
        }
        public async Task<IActionResult> RemoveFromPlayerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData.Add("error", "There is no such user");
                return RedirectToAction("Index", "RoleRequests");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Player"))
            {
                TempData.Add("error", "User is not a player!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await playerService.RemoveFromPlayerRoleAsync(user.Id);
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Player"
            };
            return RedirectToAction("RemoveFromRole", "RoleRequests", model);
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

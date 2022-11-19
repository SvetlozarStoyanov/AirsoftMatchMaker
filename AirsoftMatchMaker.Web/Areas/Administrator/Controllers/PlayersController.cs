using AirsoftMatchMaker.Core.Contracts;
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

        public PlayersController(UserManager<User> userManager, IPlayerService playerService)
        {
            this.userManager = userManager;
            this.playerService = playerService;
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
        public async Task<IActionResult> Index()
        {
            var model = await playerService.GetAllPlayersAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await playerService.GetPlayerByIdAsync(id);
            if (model == null)
            {
                TempData.Add("error", $"Player with {id} id does not exist!");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    }
}

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
    public class MatchmakersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMatchmakerService matchmakerService;
        public MatchmakersController(UserManager<User> userManager, SignInManager<User> signInManager, IMatchmakerService vendorService)
        {
            this.userManager = userManager;
            this.matchmakerService = vendorService;
        }

        public async Task<IActionResult> GrantMatchmakerRole(string userId, int roleRequestId)
        {
            await matchmakerService.CreateMatchmakerAsync(userId);
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Matchmaker",
                RoleRequestId = roleRequestId.ToString()
            };
            return RedirectToAction("GrantRole", "RoleRequests", model);
        }

        public async Task<IActionResult> RemoveFromMatchmakerRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData.Add("error", "There is no such user");
                return RedirectToAction("Index", "RoleRequests");
            }
            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains("Matchmaker"))
            {
                TempData.Add("error", "User is not a matchmaker!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await matchmakerService.RetireMatchmakerAsync(userId);

            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = userId,
                RoleName = "Matchmaker"
            };
            return RedirectToAction("RemoveFromRole", "RoleRequests", model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await matchmakerService.GetAllMatchmakersAsync();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var model = await matchmakerService.GetMatchmakerByIdAsync(id);
            return View(model);
        }

    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Matchmaker.Controllers
{
    [Area("Matchmaker")]
    [Authorize(Roles ="Matchmaker")]
    public class PlayersController : Controller
    {
        private readonly IPlayerService playerService;
        public PlayersController(IPlayerService playerService)
        {
            this.playerService = playerService;
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

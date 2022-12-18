using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    [Authorize]
    public class MatchmakersController : Controller
    {
        private readonly IMatchmakerService matchmakerService;

        public MatchmakersController(IMatchmakerService matchmakerService)
        {

            this.matchmakerService = matchmakerService;
        }

        public async Task<IActionResult> RequestMatchmakerRole()
        {
            if (User.IsInRole("Player") || User.IsInRole("Matchmaker") || User.IsInRole("Vendor"))
            {
                TempData.Add("error", "You cannot be assigned to that role!");
                return RedirectToAction("Index", "RoleRequests");
            }
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Matchmaker"
            };
            return RedirectToAction("RequestRole", "RoleRequests", model);
        }

        public async Task<IActionResult> LeaveMatchmakerRole()
        {
            if (!User.IsInRole("Matchmaker"))
            {
                TempData.Add("error", "You are not a matchmaker!");
                return RedirectToAction("Index", "RoleRequests");
            }
            await matchmakerService.RetireMatchmakerAsync(User.Id());
            RoleRequestRouteModel model = new RoleRequestRouteModel()
            {
                UserId = User.Id(),
                RoleName = "Matchmaker"
            };
            return RedirectToAction("LeaveRole", "RoleRequests", model);
        }
    }
}

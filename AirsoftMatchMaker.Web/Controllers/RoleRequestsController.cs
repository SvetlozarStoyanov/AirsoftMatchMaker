using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.RoleRequests;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    [Authorize]
    public class RoleRequestsController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IRoleService roleService;
        private readonly IRoleRequestService roleRequestService;

        public RoleRequestsController(UserManager<User> userManager, RoleManager<Role> roleManager, IRoleService roleService, IRoleRequestService roleRequestService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.roleService = roleService;
            this.roleRequestService = roleRequestService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var userRoleNames = await userManager.GetRolesAsync(user);
            var model = await roleService.GetRequestableRolesAsync(userRoleNames);
            ViewBag.UserRoles = await roleService.GetUserRolesAsync(userRoleNames);

            return View(model);
        }

        public async Task<IActionResult> RequestRole([FromQuery] RoleRequestRouteModel model)
        {
            if (!roleManager.Roles.Any(r => r.Name == model.RoleName))
            {
                TempData.Add("error", $"No role with {model.RoleName}!");
                return RedirectToAction(nameof(Index));
            }
            var roleId = roleManager.Roles.FirstOrDefault(r => r.Name == model.RoleName).Id;
            await roleRequestService.CreateRoleRequestAsync(roleId, User.Id());
            TempData.Add("success", $"Successfully requested role {model.RoleName}!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LeaveRole([FromQuery] RoleRequestRouteModel model)
        {
            var user = await userManager.GetUserAsync(User);
            await userManager.RemoveFromRoleAsync(user, model.RoleName);
            TempData.Add("success", $"You are no longer a {model.RoleName}!");

            return RedirectToAction("LogoutAndLogin","Users");
        }
    }
}

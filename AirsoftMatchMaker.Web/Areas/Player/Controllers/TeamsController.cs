using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class TeamsController : Controller
    {
        private readonly ITeamService teamService;
        private readonly ITeamRequestService teamRequestService;
        private readonly IHtmlSanitizingService htmlSanitizingService;
        public TeamsController(ITeamService teamService, ITeamRequestService teamRequestService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.teamService = teamService;
            this.teamRequestService = teamRequestService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserHasTeam = await teamService.DoesUserHaveTeamAsync(User.Id());
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

        public async Task<IActionResult> MyTeam()
        {
            if (!(await teamService.DoesUserHaveTeamAsync(User.Id())))
            {
                TempData["error"] = $"You don't have a team!";
                return RedirectToAction(nameof(Index));
            }
            var model = await teamService.GetPlayersTeamAsync(User.Id());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (await teamRequestService.DoesTheUserHaveAcceptedOrPendingTeamRequestsAsync(User.Id()))
            {
                TempData["error"] = "You can only create a team if you cancel your requests to join another!";
                return RedirectToAction("Mine", "TeamRequests");
            }
            if (await teamService.DoesUserHaveTeamAsync(User.Id()))
            {
                TempData["error"] = "Cannot create team when part of other!";
                return RedirectToAction(nameof(Index));

            }
            var model = new TeamCreateModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await teamService.DoesTeamWithSameNameExistAsync(model.Name))
            {
                TempData["error"] = "Team with that name already exists!";
                return View(model);
            }
            model = htmlSanitizingService.SanitizeObject<TeamCreateModel>(model);
            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await teamService.DoesTeamWithSameNameExistAsync(model.Name))
            {
                TempData["error"] = "Team with that name already exists!";
                return View(model);
            }
            await teamService.CreateTeamAsync(User.Id(), model);
            return RedirectToAction(nameof(MyTeam));

        }
    }
}

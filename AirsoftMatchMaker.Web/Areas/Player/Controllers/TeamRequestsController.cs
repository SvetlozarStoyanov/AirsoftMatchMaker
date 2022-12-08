using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class TeamRequestsController : Controller
    {
        private readonly ITeamRequestService teamRequestService;
        public TeamRequestsController(ITeamRequestService teamRequestService)
        {
            this.teamRequestService = teamRequestService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await teamRequestService.GetTeamRequestsForTeamByUserIdAsync(User.Id());
                ViewBag.PendingRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Pending).ToList();
                ViewBag.AcceptedJoinRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Accepted).ToList();
                ViewBag.AcceptedLeaveRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Leave && tr.TeamRequestStatus == TeamRequestStatus.Accepted).ToList();
                ViewBag.DeclinedJoinRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Declined).ToList();
                ViewBag.ViewType = "Index";
                return View();
            }
            catch (Exception ex)
            {
                TempData["error"] = "You cannot access this page!";
            }
            return RedirectToAction("Index", "Teams");
        }


        public async Task<IActionResult> Create(int teamId, string requestType)
        {
            if (!Enum.TryParse(requestType, out TeamRequestType teamRequestType))
            {
                TempData["error"] = "Invalid data!";
                return RedirectToAction(nameof(Index));
            }
            if (await teamRequestService.DoesTeamRequestAlreadyExistAsync(User.Id(), teamId))
            {
                TempData["error"] = "This request already exists!";
                return RedirectToAction("Index", "Teams");
            }
            if (!(await teamRequestService.IsTeamRequestValidAsync(User.Id(), teamId, teamRequestType)))
            {
                TempData["error"] = "Invalid team request!";
                return RedirectToAction("Index", "Teams");
            }
            try
            {
                await teamRequestService.CreateTeamRequestAsync(User.Id(), teamId, teamRequestType);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Invalid operation!";
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = $"{requestType} team requested!";
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }

        public async Task<IActionResult> Accept(int id)
        {
            if (!(await teamRequestService.CanUserAcceptOrDeclineTeamRequestAsync(User.Id(), id)))
            {
                TempData["error"] = "Invalid operation!";
                return RedirectToAction(nameof(Index));
            }
            await teamRequestService.AcceptTeamRequestAsync(id);
            TempData["success"] = "Join request accepted! Player will join your team as soon as possible!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Decline(int id)
        {
            if (!(await teamRequestService.CanUserAcceptOrDeclineTeamRequestAsync(User.Id(), id)))
            {
                TempData["error"] = "Invalid operation!";
                return RedirectToAction(nameof(Index));
            }
            await teamRequestService.DeclineTeamRequestAsync(id);
            TempData["success"] = "Join request declined!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Mine()
        {
            var model = await teamRequestService.GetTeamRequestsByUserByUserIdAsync(User.Id());
            ViewBag.PendingRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Pending).ToList();
            ViewBag.AcceptedJoinRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Accepted).ToList();
            ViewBag.AcceptedLeaveRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Leave && tr.TeamRequestStatus == TeamRequestStatus.Accepted).ToList();
            ViewBag.DeclinedJoinRequests = model.Where(tr => tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Declined).ToList();
            ViewBag.ViewType = "Mine";

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await teamRequestService.IsTeamRequestCreatedByUserAsync(User.Id(), id))
            {
                TempData["error"] = "Invalid operation!";
                return RedirectToAction(nameof(Index));
            }
            await teamRequestService.DeleteTeamRequestAsync(id);
            TempData["success"] = "Request deleted!";
            return RedirectToAction(nameof(Mine));
        }
    }
}

﻿using AirsoftMatchMaker.Core.Contracts;
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
        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
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
            if (!(await teamService.DoesPlayerHaveTeam(User.Id())))
            {
                TempData["error"] = $"You don't have a team!";
                return RedirectToAction(nameof(Index));
            }
            var model = await teamService.GetPlayersTeamAsync(User.Id());
            return View(model);
        }
    }
}

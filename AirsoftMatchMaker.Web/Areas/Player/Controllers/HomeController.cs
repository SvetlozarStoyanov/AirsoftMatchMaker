﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Web.Extensions;
using AirsoftMatchMaker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService gameService;
        private readonly ITeamService teamService;
        public HomeController(ILogger<HomeController> logger, IGameService gameService, ITeamService teamService)
        {
            _logger = logger;
            this.gameService = gameService;
            this.teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
            if (await teamService.DoesUserHaveTeamAsync(User.Id()))
            {
                var playerGamesmodel = await gameService.GetPlayersLastFinishedAndFirstUpcomingGameAsync(User.Id());
                if (playerGamesmodel == null)
                {
                    var defaultModel = await gameService.GetUpcomingGamesByDateAsync();
                    return View(defaultModel);
                }
                ViewBag.FinishedGame = playerGamesmodel.FirstOrDefault(g => g.GameStatus == GameStatus.Finished);
                ViewBag.UpcomingGame = playerGamesmodel.FirstOrDefault(g => g.GameStatus == GameStatus.Upcoming);
                return View();
            }
            var model = await gameService.GetUpcomingGamesByDateAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
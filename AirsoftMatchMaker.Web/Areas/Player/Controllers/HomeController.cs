using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
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
        private readonly IPlayerService playerService;
        private readonly IBetService betService;
        public HomeController(ILogger<HomeController> logger, IGameService gameService, ITeamService teamService, IPlayerService playerService, IBetService betService)
        {
            _logger = logger;
            this.gameService = gameService;
            this.teamService = teamService;
            this.playerService = playerService;
            this.betService = betService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            ViewBag.UserTeamId = await playerService.GetPlayersTeamIdAsync(User.Id());
            if (await teamService.DoesUserHaveTeamAsync(User.Id()))
            {
                var playerGamesmodel = await gameService.GetPlayersLastFinishedAndFirstUpcomingGameAsync(User.Id());
                if (playerGamesmodel.Count() == 0)
                {
                    var defaultModel = await gameService.GetUpcomingGamesByDateAsync();
                    ViewBag.GameIdsOfGamesUserHasBetOn = await betService.GetGamesIdsWhichUserHasBetOnAsync(User.Id());
                    return View(defaultModel);
                }

                if (playerGamesmodel.Any(g => g.GameStatus == GameStatus.Finished))
                {
                    ViewBag.FinishedGame = playerGamesmodel.FirstOrDefault(g => g.GameStatus == GameStatus.Finished);
                }

                if (playerGamesmodel.Any(g => g.GameStatus == GameStatus.Upcoming))
                {
                    ViewBag.UpcomingGame = playerGamesmodel.FirstOrDefault(g => g.GameStatus == GameStatus.Upcoming);
                }
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
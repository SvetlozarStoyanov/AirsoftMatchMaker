using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Web.Extensions;
using AirsoftMatchMaker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService gameService;
        private readonly IBetService betService;
        public HomeController(ILogger<HomeController> logger, IGameService gameService, IBetService betService)
        {
            _logger = logger;
            this.gameService = gameService;
            this.betService = betService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await gameService.GetUpcomingGamesByDateAsync();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.GameIdsOfGamesUserHasBetOn = await betService.GetGamesIdsWhichUserHasBetOnAsync(User.Id());
            }
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
using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class BetsController : Controller
    {
        private readonly IBetService betService;
        private readonly IPlayerService playerService;
        private readonly IGameService gameService;

        public BetsController(IBetService betService, IPlayerService playerService, IGameService gameService)
        {
            this.betService = betService;
            this.playerService = playerService;
            this.gameService = gameService;
        }


        public async Task<IActionResult> Create(int gameId)
        {
            if (!(await gameService.GameExistsAsync(gameId)))
            {
                TempData["error"] = "Game does not exist";
                return RedirectToAction("Index", "Games");
            }
            if (await betService.IsUserMatchmakerAsync(User.Id()))
            {
                TempData["error"] = "Users who are/were in matchmaker role cannot bet!";
                return RedirectToAction("Index", "Games");
            }
            if (await betService.IsGameFinishedAsync(gameId))
            {
                TempData["error"] = "Game has already concluded!";
                return RedirectToAction("Index", "Games");
            }
            if (!(await betService.DoesGameStillAcceptBetsAsync(gameId)))
            {
                TempData["error"] = "Game no longer accepts bets!";
                return RedirectToAction("Index", "Games");
            }
            if (await betService.HasUserAlreadyBetOnGameAsync(User.Id(), gameId))
            {
                TempData["error"] = "Cannot bet twice on the same game!";
                return RedirectToAction("Index", "Games");
            }
            if (await betService.IsUserInOneOfTheTeamsInTheGameAsync(User.Id(), gameId))
            {
                TempData["error"] = "You cannot bet on games your team participates in!";
                return RedirectToAction("Index", "Games");
            }
            var model = await betService.CreateBetCreateModelAsync(User.Id(), gameId);
            model.UserCredits = await playerService.GetPlayersAvailableCreditsAsync(User.Id());
            if (model.UserCredits < 1)
            {
                TempData["error"] = "You don't have enough credits to place a bet!";
                return RedirectToAction("Index", "Games");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BetCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await betService.CreateBetAsync(User.Id(), model);
            TempData["success"] = "Bet placed successfully!";
            return RedirectToAction("Index", "Games");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!(await betService.BetExistsAsync(id)))
            {
                TempData["error"] = "Bet does not exist!";
                return RedirectToAction(nameof(Mine));
            }
            if (!(await betService.UserCanAccessBetAsync(User.Id(), id)))
            {
                TempData["error"] = "Cannot access bet!";
                return RedirectToAction(nameof(Mine));
            }
            var model = await betService.GetBetByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await betService.BetExistsAsync(id)))
            {
                TempData["error"] = "Bet does not exist";
                return RedirectToAction(nameof(Mine));
            }
            if (!(await betService.UserCanAccessBetAsync(User.Id(), id)))
            {
                TempData["error"] = "Cannot access bet!";
                return RedirectToAction(nameof(Mine));
            }
            var gameId = await betService.GetGameIdByBetAsync(id);
            if (await betService.IsGameFinishedAsync(gameId))
            {
                TempData["error"] = "Cannot cancel bet! Game is already finished.";
                return RedirectToAction(nameof(Mine));
            }
            var model = await betService.GetBetToDeleteByIdAsync(id);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int betId, int gameId)
        {
            if (!(await betService.BetExistsAsync(betId)))
            {
                TempData["error"] = "Bet does not exist";
                return RedirectToAction(nameof(Mine));
            }
            if (!(await betService.UserCanAccessBetAsync(User.Id(), betId)))
            {
                TempData["error"] = "Cannot access bet!";
                return RedirectToAction(nameof(Mine));
            }
            if (await betService.IsGameFinishedAsync(gameId))
            {
                TempData["error"] = "Cannot cancel bet! Game is already finished.";
                return RedirectToAction(nameof(Mine));
            }
            await betService.DeleteBetAsync(betId);
            TempData["success"] = "Bet deleted successfully! Your credits have been refunded.";

            return RedirectToAction(nameof(Mine));
        }


        public async Task<IActionResult> Mine()
        {
            var model = await betService.GetUserBetsAsync(User.Id());
            return View(model);
        }
    }
}

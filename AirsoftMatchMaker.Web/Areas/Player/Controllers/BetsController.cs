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

        public BetsController(IBetService betService)
        {
            this.betService = betService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int gameId)
        {
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
            if (model == null)
            {
                TempData["error"] = "Cannot create bet!";
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
                TempData["error"] = "Bet does not exist";
                return RedirectToAction(nameof(Mine));
            }
            var model = await betService.GetBetByIdAsync(id);
            if (model.UserId != User.Id())
            {
                TempData["error"] = "Cannot access that bet!";
                return RedirectToAction(nameof(Mine));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int betId, int gameId)
        {
            if (!(await betService.BetExistsAsync(betId)))
            {
                TempData["error"] = "Bet does not exist";
                return RedirectToAction(nameof(Mine));
            }
            if (await betService.IsGameFinishedAsync(gameId))
            {
                TempData["error"] = "Cannot cancel bet! Game is already finished.";
                return RedirectToAction(nameof(Mine));
            }
            var model = await betService.GetBetToDeleteByIdAsync(betId);
            if (model.UserId != User.Id())
            {
                TempData["error"] = "Cannot access that bet!";
                return RedirectToAction(nameof(Mine));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BetDeleteModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await betService.DeleteBetAsync(model);
            return RedirectToAction(nameof(Mine));
        }

        public async Task<IActionResult> Mine()
        {
            var model = await betService.GetUserBetsAsync(User.Id());
            return View(model);
        }
    }
}

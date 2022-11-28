using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    [Authorize]
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
            if (await betService.HasUserAlreadyBetOnGameAsync(User.Id(), gameId))
            {
                TempData.Add("error", "Cannot bet twice on the same game!");
                return RedirectToAction("Index", "Games");
            }
            var model = await betService.CreateBetCreateModelAsync(User.Id(), gameId);
            if (model == null)
            {
                TempData.Add("error", "Cannot create bet!");
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
            TempData.Add("success", "Bet placed successfully!");
            return RedirectToAction("Index", "Games");
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await betService.GetBetByIdAsync(id);
            if (model == null || model.UserId != User.Id())
            {
                TempData.Add("error", "Cannot access that bet!");
                return RedirectToAction(nameof(Mine));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await betService.GetBetToDeleteByIdAsync(id);
            if (model == null || model.UserId != User.Id())
            {
                TempData.Add("error", "Cannot access that bet!");
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

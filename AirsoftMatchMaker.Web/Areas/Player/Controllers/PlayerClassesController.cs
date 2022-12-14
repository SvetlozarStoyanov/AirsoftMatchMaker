using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Player.Controllers
{
    [Area("Player")]
    [Authorize(Roles = "Player")]
    public class PlayerClassesController : Controller
    {
        private readonly IPlayerClassService playerClassService;

        public PlayerClassesController(IPlayerClassService playerClassService)
        {
            this.playerClassService = playerClassService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await playerClassService.GetAllPlayerClassesAsync();
            ViewBag.CurrentUserPlayerClassId = await playerClassService.GetPlayersPlayerClassIdByUserIdAsync(User.Id());
            return View(model);
        }

       
        public async Task<IActionResult> ChangeClass(int id)
        {
            if (!(await playerClassService.DoesPlayerClassExistAsync(id)))
            {
                TempData["error"] = "Player class does not exist!";
            }
            if (await playerClassService.IsPlayerAlreadyInPlayerClass(User.Id(), id))
            {
                TempData["error"] = "You are already assigned to that class";
            }
            else
            {
                await playerClassService.ChangePlayerClassAsync(User.Id(), id);
                TempData["success"] = "Successfully assigned to class!";
            }
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Leave(int id)
        //{
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

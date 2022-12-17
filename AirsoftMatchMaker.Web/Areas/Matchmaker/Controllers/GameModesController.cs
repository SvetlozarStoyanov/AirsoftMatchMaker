﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Areas.Matchmaker.Controllers
{
    [Area("Matchmaker")]
    [Authorize(Roles = "Matchmaker")]
    public class GameModesController : Controller
    {
        private readonly IGameModeService gameModeService;
        private readonly IHtmlSanitizingService htmlSanitizingService;

        public GameModesController(IGameModeService gameModeService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.gameModeService = gameModeService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await gameModeService.GetAllGameModesAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await gameModeService.GetGameModeByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Game mode with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new GameModeCreateModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameModeCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await gameModeService.DoesGameModeExistAsync(model.Name))
            {
                TempData["error"] = $"Game mode with name {model.Name} already exists!";
                return View(model);
            }

            model = htmlSanitizingService.SanitizeObject<GameModeCreateModel>(model);
            ModelState.Clear();
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await gameModeService.DoesGameModeExistAsync(model.Name))
            {
                TempData["error"] = $"Game mode with name {model.Name} already exists!";
                return View(model);
            }
            await gameModeService.CreateGameModeAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}

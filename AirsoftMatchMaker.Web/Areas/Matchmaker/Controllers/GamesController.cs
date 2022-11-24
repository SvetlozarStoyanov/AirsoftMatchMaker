﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AirsoftMatchMaker.Web.Areas.Matchmaker.Controllers
{
    [Area("Matchmaker")]
    [Authorize(Roles = "Matchmaker")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await gameService.GetAllGamesAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var model = await gameService.GetGameByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Game with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult SelectGameDate()
        {
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(7);
            var dates = Enumerable.Range(0, (int)(endDate - startDate).TotalDays + 1)
                      .Select(x => startDate.AddDays(x).ToString())
                      .ToList();
           
            var selectDateModel = new GameSelectDateModel()
            {
                Dates = dates
            };
            return View(selectDateModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(GameSelectDateModel selectDateModel)
        {
            var createModel = await gameService.CreateGameModel(selectDateModel.DateTime);
            return View(createModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateModel model)
        {
            if (!ModelState.IsValid || model.TeamRedId == model.TeamBlueId)
            {
                var createModel = await gameService.CreateGameModel(model.DateString);
                return View(createModel);
            }
            await gameService.CreateGameAsync(User.Id(), model);
            return RedirectToAction(nameof(Index));
        }
    }
}

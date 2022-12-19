using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class ClothesController : Controller
    {
        private readonly IClothingService clothingService;
        private IHtmlSanitizingService htmlSanitizingService;
        public ClothesController(IClothingService clothingService, IHtmlSanitizingService htmlSanitizingService)
        {
            this.clothingService = clothingService;
            this.htmlSanitizingService = htmlSanitizingService;
        }

        public async Task<IActionResult> Index([FromQuery]ClothesQueryModel model)
        {
            model.SearchTerm = htmlSanitizingService.SanitizeStringProperty(model.SearchTerm);
            var queryResult = await clothingService.GetAllClothesAsync
                (
                    model.ClothingColor,
                    model.Sorting,
                    model.SearchTerm,
                    model.ClothesPerPage,
                    model.CurrentPage
                );
            model.Clothes = queryResult.Clothes;
            model.Colors = queryResult.Colors;
            model.SortingOptions = queryResult.SortingOptions;
            model.ClothesCount = queryResult.ClothesCount;
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await clothingService.GetClothingByIdAsync(id);
            if (model == null)
            {
                TempData["error"] = $"Clothing with {id} id does not exist!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

     
    }
}

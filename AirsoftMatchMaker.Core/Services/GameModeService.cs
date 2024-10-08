using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameModeService : IGameModeService
    {
        private readonly IUnitOfWork unitOfWork;
        public GameModeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> GameModeExistsAsync(int id)
        {
            var gameMode = await unitOfWork.GameModeRepository.GetByIdAsync(id);
            return gameMode != null;
        }
        public async Task<bool> IsGameModeNameAlreadyTaken(string gameModeName)
        {
            return await unitOfWork.GameModeRepository.AllReadOnly()
                .AnyAsync(gm => gm.Name == gameModeName);
        }

        public async Task<GameModesQueryModel> GetAllGameModesAsync(
            string? searchTerm = null,
            GameModeSorting sorting = GameModeSorting.Newest,
            int gameModesPerPage = 6,
            int currentPage = 1
            )
        {
            var gameModes = await unitOfWork.GameModeRepository.AllReadOnly()
                .Include(gm => gm.Maps)
                .Include(gm => gm.Games)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                gameModes = gameModes.Where(gm => gm.Name.ToLower().Contains(searchTerm.ToLower()) || gm.Description.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (sorting)
            {
                case GameModeSorting.Newest:
                    gameModes = gameModes.OrderByDescending(gm => gm.Id).ToList();
                    break;
                case GameModeSorting.Oldest:
                    gameModes = gameModes.OrderBy(gm => gm.Id).ToList();
                    break;
                case GameModeSorting.GamesPlayedAscending:
                    gameModes = gameModes.OrderBy(gm => gm.Games.Count).ToList();
                    break;
                case GameModeSorting.GamesPlayedDescending:
                    gameModes = gameModes.OrderByDescending(gm => gm.Games.Count).ToList();
                    break;
                case GameModeSorting.MapCountAscending:
                    gameModes = gameModes.OrderBy(gm => gm.Maps.Count).ToList();
                    break;
                case GameModeSorting.MapCountDescending:
                    gameModes = gameModes.OrderByDescending(gm => gm.Maps.Count).ToList();
                    break;
            }
            var filteredGameModes = gameModes
                .Skip((currentPage - 1) * gameModesPerPage)
                .Take(gameModesPerPage)
                .Select(gm => new GameModeListModel()
                {
                    Id = gm.Id,
                    Name = gm.Name,
                    Description = gm.Description,
                    MapsCount = gm.Maps.Count(),
                    PointsToWin = gm.PointsToWin
                })
                .ToList();
            GameModesQueryModel model = CreateGameModesQueryModel();
            model.GameModes = filteredGameModes;
            model.GameModesCount = gameModes.Count;
            return model;
        }


        public async Task<GameModeViewModel> GetGameModeByIdAsync(int id)
        {
            var gameMode = await unitOfWork.GameModeRepository.AllReadOnly()
                .Where(gm => gm.Id == id)
                .Include(gm => gm.Maps)
                .Include(gm => gm.Games)
                .Select(gm => new GameModeViewModel()
                {
                    Id = gm.Id,
                    Name = gm.Name,
                    Description = gm.Description,
                    PointsToWin = gm.PointsToWin,
                    Maps = gm.Maps.Select(m => new MapMinModel()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        ImageUrl = m.ImageUrl,
                        Terrain = m.Terrain
                    })
                    .ToList(),
                    Games = gm.Games.OrderByDescending(g => g.Date)
                    .Where(g => g.GameStatus == GameStatus.Finished)
                    .Take(6)
                    .Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Date = g.Date.ToShortDateString(),
                        Result = g.Result
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();
            return gameMode;
        }

        public async Task CreateGameModeAsync(GameModeCreateModel model)
        {
            var gameMode = new GameMode()
            {
                Name = model.Name,
                Description = model.Description,
                PointsToWin = model.PointsToWin
            };

            await unitOfWork.GameModeRepository.AddAsync(gameMode);
            await unitOfWork.SaveChangesAsync();
        }


        private GameModesQueryModel CreateGameModesQueryModel()
        {
            var model = new GameModesQueryModel();
            model.SortingOptions = Enum.GetValues<GameModeSorting>().ToList();
            return model;
        }


    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameModeService : IGameModeService
    {
        private readonly IRepository repository;
        public GameModeService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<GameModeListModel>> GetAllGameModesAsync()
        {
            var gameModes = await repository.AllReadOnly<GameMode>()
                .Select(gm => new GameModeListModel()
                {
                    Id = gm.Id,
                    Name = gm.Name,
                    Description = gm.Description,
                    MapsCount = gm.Maps.Count(),
                    PointsToWin = gm.PointsToWin
                })
                .ToListAsync();
            return gameModes;
        }

        public async Task<GameModeViewModel> GetGameModeByIdAsync(int id)
        {
            var gameMode = await repository.AllReadOnly<GameMode>()
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
    }
}

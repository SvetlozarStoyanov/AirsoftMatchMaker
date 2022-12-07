using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
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
                        ImageUrl = m.ImageUrl
                    })
                    .ToList(),
                    Games = gm.Games.Select( g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();
            return gameMode;
        }
    }
}

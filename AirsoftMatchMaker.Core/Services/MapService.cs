using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class MapService : IMapService
    {
        private readonly IRepository repository;
        public MapService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<MapListModel>> GetAllMapsAsync()
        {
            var maps = await repository.AllReadOnly<Map>()
                .Include(m => m.GameMode)
                .Include(m => m.Games)
                .Select(m => new MapListModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    ImageUrl = m.ImageUrl,
                    AverageEngagementDistance = m.AverageEngagementDistance,
                    Terrain = m.Terrain,
                    Mapsize = m.Mapsize,
                    GameModeId = m.GameModeId,
                    GameModeName = m.GameMode.Name,
                    GamesPlayed = m.Games.Count
                })
                .ToListAsync();
            return maps;
        }

        public async Task<MapViewModel> GetMapByIdAsync(int id)
        {
            var map = await repository.AllReadOnly<Map>()
                .Where(m => m.Id == id)
                .Include(m => m.GameMode)
                .Include(m => m.Games)
                .ThenInclude(g => g.TeamBlue)
                .Include(m => m.Games)
                .ThenInclude(g => g.TeamRed)
                .Select(m => new MapViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    AverageEngagementDistance = m.AverageEngagementDistance,
                    Terrain = m.Terrain,
                    Mapsize = m.Mapsize,
                    GameModeId = m.GameModeId,
                    GameModeName = m.GameMode.Name,
                    Games = m.Games
                    .OrderByDescending(g => g.Date)
                    .Where(g => g.GameStatus == GameStatus.Finished)
                    .Take(6)
                    .Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Date = g.Date.ToShortDateString(),
                        Result = g.Result != null ? g.Result : "Not played yet",
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();
            return map;
        }
    }
}

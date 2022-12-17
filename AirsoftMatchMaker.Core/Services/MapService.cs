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

        public async Task<bool> MapAlreadyExists(string mapName)
        {
            return await repository.AllReadOnly<Map>()
                .AnyAsync(m => m.Name == mapName);
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




        public async Task<MapCreateModel> CreateMapCreateModelAsync()
        {
            var model = new MapCreateModel();
            model.TerrainTypes = Enum.GetValues<TerrainType>().ToList();
            model.AverageEngagementDistances = Enum.GetValues<AverageEngagementDistance>().ToList();
            model.Mapsizes = Enum.GetValues<Mapsize>().ToList();
            var gameModes = await repository.AllReadOnly<GameMode>()
                .Select(gm => new
                {
                    Id = gm.Id,
                    Name = gm.Name
                })
                .ToListAsync();
            model.GameModeIds = gameModes.Select(gm => gm.Id).ToList();
            model.GameModeNames = gameModes.Select(gm => gm.Name).ToList();
            return model;
        }


        public async Task CreateMapAsync(MapCreateModel model)
        {
            var map = new Map()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Terrain = model.TerrainType,
                Mapsize = model.Mapsize,
                AverageEngagementDistance= model.AverageEngagementDistance,
                GameModeId = model.GameModeId,
            };
            await repository.AddAsync<Map>(map);
            await repository.SaveChangesAsync();
        }
    }
}

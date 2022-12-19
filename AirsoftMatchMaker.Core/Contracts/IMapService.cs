using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IMapService
    {
        /// <summary>
        /// Returns true if <see cref="Map"/> with given exists name exists, false otherwise
        /// 
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        Task<bool> MapAlreadyExists(string mapName);


        Task<MapsQueryModel> GetAllMapsAsync(
            string? searchTerm = null,
            string? gameModeName = null,
            MapSorting sorting = MapSorting.GamesPlayedDescending,
            int mapsPerPage = 6,
            int currentPage = 1
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="MapViewModel"/></returns>
        Task<MapViewModel> GetMapByIdAsync(int id);

        /// <summary>
        /// Creates a <see cref="MapCreateModel"/> and fills collections with required information
        /// </summary>
        /// <returns><see cref="MapCreateModel"/></returns>
        Task<MapCreateModel> CreateMapCreateModelAsync();

        /// <summary>
        /// Creates a <see cref="Map"/> from <see cref="MapCreateModel"/> and adds it to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateMapAsync(MapCreateModel model);
    }
}

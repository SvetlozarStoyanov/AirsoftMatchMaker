using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IMapService
    {
        /// <summary>
        /// Returns true if <see cref="Map"/> with <paramref name="id"/> exists, false otherwise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> MapExistsAsync(int id);

        /// <summary>
        /// Returns true if <see cref="Map"/> with given exists name exists, false otherwise
        /// 
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        Task<bool> IsMapNameAlreadyTaken(string mapName);

        /// <summary>
        /// Returns <see cref="MapsQueryModel"/> which contains <see cref="IEnumerable{T}"/> 
        /// of maps which match the given criteria
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="gameModeName"></param>
        /// <param name="sorting"></param>
        /// <param name="mapsPerPage"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
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

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

        /// <summary>
        /// Returns all maps
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<MapListModel>> GetAllMapsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="MapViewModel"/></returns>
        Task<MapViewModel> GetMapByIdAsync(int id);


        Task<MapCreateModel> CreateMapCreateModelAsync();


        Task CreateMapAsync(MapCreateModel model);
    }
}

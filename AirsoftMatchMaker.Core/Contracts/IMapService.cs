using AirsoftMatchMaker.Core.Models.Maps;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IMapService
    {
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
    }
}

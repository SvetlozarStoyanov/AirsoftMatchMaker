using AirsoftMatchMaker.Core.Models.Games;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IGameService
    {
        /// <summary>
        /// Returns all games
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<GameListModel>> GetAllGamesAsync();

        /// <summary>
        /// Returns game with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="GameViewModel"/></returns>
        Task<GameViewModel> GetGameByIdAsync(int id);

        Task<GameCreateModel> CreateGameModel(string dateTimeString);

        Task CreateGameAsync(string matchmakerUserId, GameCreateModel model);
    }
}

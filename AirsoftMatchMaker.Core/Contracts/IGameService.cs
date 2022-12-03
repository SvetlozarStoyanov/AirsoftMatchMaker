using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

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

        /// <summary>
        /// Creates a <see cref="GameSelectDateModel model"/> which contains a collection of the next available
        /// dates where at least one <see cref="Map map"/> and two <see cref="Team team"/> are available to be booked
        /// for a game.
        /// </summary>
        /// <returns><see cref="GameSelectDateModel model"/></returns>
        Task<GameSelectDateModel> GetNextSevenAvailableDatesAsync();

        /// <summary>
        /// Creates the <see cref="GameCreateModel model"/> with the appropriate available teams and maps.
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns><see cref="GameCreateModel model"/></returns>
        Task<GameCreateModel> CreateGameModelAsync(string dateTimeString);


        /// <summary>
        /// Creates a <see cref="Game game"/> and adds it to the database
        /// </summary>
        /// <param name="matchmakerUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateGameAsync(string matchmakerUserId, GameCreateModel model);


        /// <summary>
        /// Returns games that will take place today.
        /// </summary>
        /// <returns><see cref="IEnumerable<<see cref="GamePartialModel"/>>"/></returns>
        Task<IEnumerable<GamePartialModel>> GetUpcomingGamesByDateAsync(DateTime dateTime);
    }
}

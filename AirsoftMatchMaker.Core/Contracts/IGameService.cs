using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IGameService
    {

        /// <summary>
        /// Checks if game exists returns true if it does, othewise returns false
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> GameExistsAsync(int id);

        /// <summary>
        /// Compares the <see cref="Player"/> count of two teams returns true if they are close 
        /// otherwise returns false.
        /// </summary>
        /// <param name="teamRedId"></param>
        /// <param name="teamBlueId"></param>
        /// <returns></returns>
        Task<bool> AreTeamPlayerCountsSimilarAsync(int teamRedId, int teamBlueId);

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
        /// dates where at least one <see cref="Map"/> and two <see cref="Team"/> are available to be booked
        /// for a game.
        /// </summary>
        /// <returns><see cref="GameSelectDateModel model"/></returns>
        Task<GameSelectDateModel> GetNextSevenAvailableDatesAsync();

        /// <summary>
        /// Creates the <see cref="GameCreateModel model"/> with the appropriate available teams and maps.
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns><see cref="GameCreateModel"/></returns>
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
        /// <returns><see cref="IEnumerable<<see cref="GameListModel"/>>"/></returns>
        Task<IEnumerable<GameListModel>> GetUpcomingGamesByDateAsync();


        Task<IEnumerable<GameListModel>> GetPlayersLastFinishedAndFirstUpcomingGameAsync(string userId);
    }
}

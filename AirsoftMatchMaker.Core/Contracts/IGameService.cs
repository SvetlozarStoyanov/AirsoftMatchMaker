using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

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
        /// Returns true if <see cref="Game.Matchmaker"/>'s <see cref="Matchmaker.UserId"/>  is equal to given userId , otherwise returns false.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> GameCanBeFinalisedAsync(string userId, int gameId);

        /// <summary>
        /// Returns true if <see cref="Game"/> is finished but it has no <see cref="Game.Result"/> yet, otherwise returns false
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> GameIsFinishedButNotFinalisedAsync(int id);

        /// <summary>
        /// Returns <see cref="GamesQueryModel"/> filled with games which match the given criteria.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="gameModeName"></param>
        /// <param name="gameStatus"></param>
        /// <param name="sorting"></param>
        /// <param name="gamesPerPage"></param>
        /// <param name="currentPage"></param>
        /// <returns><see cref="GamesQueryModel"/></returns>
        Task<GamesQueryModel> GetAllGamesAsync(
            string? teamName,
            string? gameModeName,
            GameStatus? gameStatus,
            GameSorting sorting,
            int gamesPerPage = 6,
            int currentPage = 1);

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
        Task<GameCreateModel> CreateGameCreateModelAsync(string dateTimeString);


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

        /// <summary>
        /// Returns <see cref="Player"/>'s most recent finished and first upcoming <see cref="Game"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<GameListModel>> GetPlayersLastFinishedAndFirstUpcomingGameAsync(string userId);

        Task<GamesMatchmakerQueryModel> GetAllGamesForAdminAndMatchmakerAsync(
            int? matchmakerId,
            MatchmakerGameStatus? status,
            GameSorting sorting,
            int gamesPerPage = 6,
            int currentPage = 1
            );

        /// <summary>
        /// Creates a <see cref="GameFinaliseModel"/> which allows the user to input the result of a game
        /// limited by the <see cref="GameFinaliseModel.GameModeMaxPoints"/> of the model.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GameFinaliseModel> CreateGameFinaliseModelAsync(int id);

        /// <summary>
        /// Takes the <see cref="GameFinaliseModel.TeamRedPoints"/> and <see cref="GameFinaliseModel.TeamBluePoints"/>
        /// and forms the result of a game
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task FinalizeGameAsync(GameFinaliseModel model);
    }
}

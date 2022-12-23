using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBetService
    {
        /// <summary>
        /// Returns true if <see cref="Bet"/> exists, otherwise returns false
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> BetExistsAsync(int id);
        /// <summary>
        /// Returns true if <see cref="Bet.UserId"/> is equal to <paramref name="userId"/>, false otherwise
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="betId"></param>
        /// <returns></returns>
        Task<bool> UserCanAccessBetAsync(string userId, int betId);

        /// <summary>
        /// Checks if the <see cref="User user"/> has already bet on <see cref="Game game"/> with given Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> HasUserAlreadyBetOnGameAsync(string userId, int gameId);

        /// <summary>
        /// Checks if the <see cref="User"/> is in one of the teams.
        /// If he is returns false, otherwise returns true.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> IsUserInOneOfTheTeamsInTheGameAsync(string userId, int gameId);

        /// <summary>
        /// Returns true if <see cref="Game"/> is <see cref="GameStatus.Finished"/>, false otherwise
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<bool> IsGameFinishedAsync(int gameId);

        /// <summary>
        /// Returns true if <see cref="Game.Date"/> is after current date , otherwise returns false
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<bool> DoesGameStillAcceptBetsAsync(int gameId);

        /// <summary>
        /// Returns true if <see cref="User"/> is in matchmaker or administrator role, otherwise false
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> IsUserMatchmakerAsync(string userId);

        /// <summary>
        /// Returns <see cref="Bet.GameId"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="int"/></returns>
        Task<int> GetGameIdByBetAsync(int id);

        /// <summary>
        /// Returns all bets created by <see cref="User"/> with given id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="IEnumerable{BetListModel}"/></returns>
        Task<IEnumerable<BetListModel>> GetUserBetsAsync(string userId);

        /// <summary>
        /// Creates <see cref="BetCreateModel model"/> with given gameId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns><see cref="BetCreateModel"/></returns>
        Task<BetCreateModel> CreateBetCreateModelAsync(string userId, int gameId);

        /// <summary>
        /// Creates a <see cref="Bet bet "/> from given <see cref="BetCreateModel model"/> and adds it to the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateBetAsync(string userId, BetCreateModel model);

        /// <summary>
        /// Returns <see cref="BetViewModel model"/> by given id if it exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BetViewModel> GetBetByIdAsync(int id);

        /// <summary>
        /// Returns <see cref="BetDeleteModel model"/> by given bet id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="BetDeleteModel model"/></returns>
        Task<BetDeleteModel> GetBetToDeleteByIdAsync(int id);

        /// <summary>
        /// Deletes a <see cref="Bet bet "/> with given id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteBetAsync(int id);

        /// <summary>
        /// Pays out the profit to the bettors who correctly predicted the result of the<see cref= "Game game" /> with given id.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task PayoutBetsByGameIdAsync(int gameId);

    }
}

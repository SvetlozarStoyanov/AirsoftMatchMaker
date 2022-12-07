using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBetService
    {
        /// <summary>
        /// Returns true if <see cref="Bet"/> exists, otherwise returns false
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool>BetExistsAsync(int id);
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

        Task<bool> IsGameFinishedAsync(int gameId);
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
        /// Also adjusts the betting line of the game.
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
        /// Also adjusts the betting line of the game.
        /// </summary>
        /// <param name="betId"></param>
        /// <returns></returns>
        Task DeleteBetAsync(BetDeleteModel model);

    }
}

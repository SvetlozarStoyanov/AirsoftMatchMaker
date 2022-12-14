using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBackgroundBetService
    {
        /// <summary>
        /// Returns the ids of <see cref="Game"/> who have been finished but whose <see cref="Game.Bets"/> have not
        /// been paid out yet
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<int>> GetAllFinalisedGameIdsWithUnpaidBetsAsync();

        /// <summary>
        /// Pays out the profit to the bettors who correctly predicted the result of the <see cref="Game game"/> with given id.
        /// </summary>
        /// <returns></returns>
        Task PayoutBetsByGameIdAsync(int gameId);
    }
}

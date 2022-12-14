using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBackgroundGameService
    {
        /// <summary>
        /// Marks every <see cref="GameStatus.Upcoming"/> <see cref="Game"/> , whose datetime is 5 hours or more older than the given <see cref="DateTime"/>
        /// , as <see cref="GameStatus.Finished"/>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task MarkGamesAsFinishedAsync(DateTime dateTime);

        /// <summary>
        /// Returns <see cref="IEnumerable{T}"/> of <see cref="Game.Id"/> whose odds are not updated.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<int>> GetGameIdsOfGamesWithNotUpDoDateOddsAsync();

        /// <summary>
        /// Calculates betting odds of <see cref="Game"/> with given id , 
        /// and changes it's <see cref="Game.OddsAreUpdated"/> to true.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task CalculateBettingOddsAsync(int gameId);
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Entities;
namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IGameSimulationService
    {
        /// <summary>
        /// Determines which games to simulate. Any game which has a date older than the given one will be simulated.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> FindGamesToSimulateAsync(DateTime dateTime);

        /// <summary>
        /// Simulates game with given Id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task SimulateGameAsync(int gameId);
    }
}

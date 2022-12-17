using AirsoftMatchMaker.Core.Models.Matchmakers;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IMatchmakerService
    {
        /// <summary>
        /// Returns <see cref="Matchmaker.Id"/> by given User Id,
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="int"/></returns>
        Task<int> GetMatchmakerIdAsync(string userId);

        /// <summary>
        /// Checks for matchmaker with given userId and marks them as active, 
        /// if there is no matchmaker with this id it creates a new one and marks them as active.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateMatchmakerAsync(string userId);

        /// <summary>
        /// Checks for matchmaker with given userId and marks them as inactive,
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RetireMatchmakerAsync(string userId);

        /// <summary>
        /// Returns all vendors
        /// </summary>
        /// <returns><see cref="IEnumerable{typeof(MatchmakerListModel)}"/></returns>
        Task<IEnumerable<MatchmakerListModel>> GetAllMatchmakersAsync();

        /// <summary>
        /// Returns vendor by given id
        /// </summary>
        /// <paramt type="int" name="id"></param>
        /// <returns><see cref="MatchmakerViewModel"/></returns>
        Task<MatchmakerViewModel> GetMatchmakerByIdAsync(int id);
    }
}

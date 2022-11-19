using AirsoftMatchMaker.Core.Models.Matchmakers;
using AirsoftMatchMaker.Core.Models.Vendors;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IMatchmakerService
    {
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

using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamService
    {

        /// <summary>
        /// Returns true if user has a team, false otherwise.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> DoesUserHaveTeamAsync(string userId);

        Task<bool> DoesTeamWithSameNameExistAsync(string name);

        /// <summary>
        /// Returns all Teams
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<TeamListModel>> GetAllTeamsAsync();

        /// <summary>
        /// Returns Team by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TeamViewModel"/></returns>
        Task<TeamViewModel> GetTeamByIdAsync(int id);

        /// <summary>
        /// Returns Player's team with given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TeamViewModel> GetPlayersTeamAsync(string userId);

        /// <summary>
        /// Creates a new <see cref="Team"/> and adds the creator to its players
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateTeamAsync(string userId, TeamCreateModel model);
    }
}

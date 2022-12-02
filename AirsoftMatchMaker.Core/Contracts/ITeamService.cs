using AirsoftMatchMaker.Core.Models.Teams;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamService
    {
       /// <summary>
       /// Returns true if player has a team , false otherwise.
       /// </summary>
       /// <param name="userId"></param>
       /// <returns><see cref="bool"/></returns>
        Task<bool> DoesPlayerHaveTeam(string userId);

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
    }
}

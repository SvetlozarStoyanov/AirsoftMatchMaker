using AirsoftMatchMaker.Core.Models.Teams;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamService
    {
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


        Task<TeamViewModel> GetPlayersTeamAsync(string userId);
    }
}

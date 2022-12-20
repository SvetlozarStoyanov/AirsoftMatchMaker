using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamService
    {

        /// <summary>
        /// Returns true if <see cref="Team"/> with <paramref name="id"/> exists, returns false otherwise
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> TeamExistsAsync(int id);

        /// <summary>
        /// Returns true if user has a team, false otherwise.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="bool"/><see cref="bool"/></returns>
        Task<bool> DoesUserHaveTeamAsync(string userId);

        /// <summary>
        /// Returns true if <see cref="Team"/> with <paramref name="name"/> exists, returns false otherwise
        /// </summary>
        /// <param name="name"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> DoesTeamWithSameNameExistAsync(string name);


        Task<TeamsQueryModel> GetAllTeamsAsync(
            string? searchTerm = null,
            TeamSorting sorting = TeamSorting.Newest,
            int teamsPerPage = 6,
            int currentPage = 1
            );

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

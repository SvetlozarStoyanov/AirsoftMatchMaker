using AirsoftMatchMaker.Core.Models.TeamRequests;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamRequestService
    {
        /// <summary>
        /// Checks if the request has been created by the same user who is trying to manipulate it.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamRequestId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> IsTeamRequestCreatedByUserAsync(string userId, int teamRequestId);

        /// <summary>
        /// Checks if player is already accepted by other team
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> IsTeamRequestValidAsync(string userId, int teamId, TeamRequestType teamRequestType);

        /// <summary>
        /// Checks if team request already exists.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> DoesTeamRequestAlreadyExistAsync(string userId, int teamId);

        /// <summary>
        /// Checks if the <see cref="User"/> trying to accept or decline the request is a part
        /// of the team
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamRequestId"></param>
        /// <returns></returns>
        Task<bool> CanUserAcceptOrDeclineTeamRequestAsync(string userId, int teamRequestId);

        Task<bool> DoesTheUserHaveAcceptedOrPendingTeamRequestsAsync(string userId);

        /// <summary>
        /// Returns the Team Requests for a given team. Only a member of the team can see them.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="IEnumerable{TeamRequestListModel}"/></returns>
        Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsForTeamByUserIdAsync(string userId);

        /// <summary>
        /// Returns the Team Requests created by a user with userId. Only that user can see them.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="IEnumerable{TeamRequestListModel}"/></returns>
        Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsByUserByUserIdAsync(string userId);


        /// <summary>
        /// Creates team request by given userId,teamId,teamRequestType.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="teamId"></param>
        /// <param name="teamRequestType"></param>
        /// <returns></returns>
        Task CreateTeamRequestAsync(string userId, int teamId, TeamRequestType teamRequestType);

        /// <summary>
        /// Accepts a team request with given Id and assigns player to team. Deletes all other join team requests by same user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task AcceptTeamRequestAsync(int id);

        /// <summary>
        /// Declines a team request with given Id .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeclineTeamRequestAsync(int id);

        /// <summary>
        /// Deletes team request from the database. Only user who created it can do this.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTeamRequestAsync(int id);
    }
}

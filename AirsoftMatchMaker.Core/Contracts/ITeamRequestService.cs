using AirsoftMatchMaker.Core.Models.TeamRequests;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface ITeamRequestService
    {
        Task<bool> IsTeamRequestCreatedByUserAsync(string userId, int teamRequestId);

        Task<bool> IsPlayerEligibleToJoinTeamAsync(string userId, int teamId);

        Task<bool> DoesTeamRequestAlreadyExistAsync(string userId, int teamId);


        Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsForTeamByUserIdAsync(string userId);


        Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsByUserByUserIdAsync(string userId);



        Task CreateTeamRequestAsync(string userId, int teamId, TeamRequestType teamRequestType);


        Task AcceptTeamRequestAsync(int id);


        Task DeclineTeamRequestAsync(int id);


        Task DeleteTeamRequestAsync(int id);
    }
}

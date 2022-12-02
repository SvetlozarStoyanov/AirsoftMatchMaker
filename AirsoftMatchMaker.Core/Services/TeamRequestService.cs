using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.TeamRequests;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class TeamRequestService : ITeamRequestService
    {
        private readonly IRepository repository;
        public TeamRequestService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<bool> IsTeamRequestCreatedByUserAsync(string userId, int teamRequestId)
        {
            var playerId = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
            var teamRequest = await repository.GetByIdAsync<TeamRequest>(teamRequestId);
            return teamRequest.PlayerId == playerId;
        }

        public async Task<bool> IsTeamRequestValidAsync(string userId, int teamId, TeamRequestType teamRequestType)
        {
            var player = await repository.All<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();
            switch (teamRequestType)
            {
                case TeamRequestType.Join:
                    if (player.TeamId != null || await HasOtherTeamHasAlreadyAcceptedPlayer(player.Id))
                        return false;
                    break;
                case TeamRequestType.Leave:
                    if (player.TeamId != teamId)
                        return false;
                    break;
            }

            return true;
        }

        public async Task<bool> DoesTeamRequestAlreadyExistAsync(string userId, int teamId)
        {
            var playerId = await repository.All<Player>()
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();
            return await repository.AllReadOnly<TeamRequest>()
                .AnyAsync(tr => tr.PlayerId == playerId && tr.TeamId == teamId);
        }

        public async Task<bool> CanUserAcceptOrDeclineTeamRequestAsync(string userId, int teamRequestId)
        {
            var teamRequest = await repository.AllReadOnly<TeamRequest>()
                .Where(tr => tr.Id == teamRequestId)
                .Include(tr => tr.Team)
                .ThenInclude(t => t.Players)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync();

            return teamRequest.Team.Players.Any(p => p.User.Id == userId);
        }

        public async Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsForTeamByUserIdAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId && p.IsActive)
                .Select(p => new { Id = p.Id, TeamId = p.TeamId })
                .FirstOrDefaultAsync();
            if (player == null)
            {
                throw new ArgumentNullException("Player does not exist or is not active!");
            }
            if (player.TeamId == null)
            {
                throw new ArgumentNullException("Team id is null!");
            }

            var teamRequests = await repository.AllReadOnly<TeamRequest>()
                .Where(tr => tr.TeamId == player.TeamId)
                .Select(tr => new TeamRequestListModel()
                {
                    Id = tr.Id,
                    PlayerId = tr.PlayerId,
                    Player = new PlayerMinModel()
                    {
                        Id = tr.PlayerId,
                        UserName = tr.Player.User.UserName,
                        SkillLevel = tr.Player.SkillLevel,
                        PlayerClassName = tr.Player.PlayerClass.Name
                    },
                    TeamId = player.TeamId.Value,
                    TeamName = tr.Team.Name,
                    TeamRequestType = tr.TeamRequestType,
                    TeamRequestStatus = tr.TeamRequestStatus
                })
                .ToListAsync();
            return teamRequests;
        }

        public async Task<IEnumerable<TeamRequestListModel>> GetTeamRequestsByUserByUserIdAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
              .Where(p => p.UserId == userId && p.IsActive)
              .Select(p => new { Id = p.Id, TeamId = p.TeamId })
              .FirstOrDefaultAsync();
            if (player == null)
            {
                throw new ArgumentNullException("Player does not exist or is not active!");
            }
            var teamRequests = await repository.AllReadOnly<TeamRequest>()
                .Where(tr => tr.PlayerId == player.Id)
                .Select(tr => new TeamRequestListModel()
                {
                    Id = tr.Id,
                    PlayerId = tr.PlayerId,
                    Player = new PlayerMinModel()
                    {
                        Id = tr.PlayerId,
                        UserName = tr.Player.User.UserName,
                        SkillLevel = tr.Player.SkillLevel,
                        PlayerClassName = tr.Player.PlayerClass.Name
                    },
                    TeamId = tr.TeamId,
                    TeamName = tr.Team.Name,
                    TeamRequestType = tr.TeamRequestType,
                    TeamRequestStatus = tr.TeamRequestStatus
                })
                .ToListAsync();
            return teamRequests;
        }

        public async Task CreateTeamRequestAsync(string userId, int teamId, TeamRequestType teamRequestType)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();
            var team = await repository.GetByIdAsync<Team>(teamId);
            if (team == null)
            {
                throw new ArgumentNullException("Team does not exist!");
            }
            if (player == null)
            {
                throw new ArgumentNullException("Player does not exist!");
            }
            var teamRequest = new TeamRequest()
            {
                PlayerId = player.Id,
                TeamId = teamId,
                TeamRequestType = teamRequestType,
            };
            if (teamRequestType == TeamRequestType.Leave)
                teamRequest.TeamRequestStatus = TeamRequestStatus.Accepted;

            await repository.AddAsync<TeamRequest>(teamRequest);
            await repository.SaveChangesAsync();
        }

        public async Task AcceptTeamRequestAsync(int id)
        {
            var teamRequest = await repository.GetByIdAsync<TeamRequest>(id);
            await DeleteOtherTeamRequests(teamRequest.Id, teamRequest.PlayerId);
            teamRequest.TeamRequestStatus = TeamRequestStatus.Accepted;
            await repository.SaveChangesAsync();
        }



        public async Task DeclineTeamRequestAsync(int id)
        {
            var teamRequest = await repository.GetByIdAsync<TeamRequest>(id);
            teamRequest.TeamRequestStatus = TeamRequestStatus.Declined;
            await repository.SaveChangesAsync();
        }

        public async Task DeleteTeamRequestAsync(int id)
        {
            await repository.DeleteAsync<TeamRequest>(id);
            await repository.SaveChangesAsync();
        }

        private async Task<bool> HasOtherTeamHasAlreadyAcceptedPlayer(int playerId)
        {
            var teamRequests = await repository.AllReadOnly<TeamRequest>()
                .Where(tr => tr.PlayerId == playerId && tr.TeamRequestType == TeamRequestType.Join && tr.TeamRequestStatus == TeamRequestStatus.Accepted)
                .ToListAsync();
            return teamRequests.Any();
        }

        private async Task DeleteOtherTeamRequests(int playerId, int teamRequestId)
        {
            var teamRequests = await repository.All<TeamRequest>()
                .Where(tr => tr.PlayerId == playerId && tr.Id != teamRequestId)
                .ToListAsync();
            repository.DeleteRange<TeamRequest>(teamRequests);
        }


    }
}

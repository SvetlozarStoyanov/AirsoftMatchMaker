using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class BackgroundTeamService : IBackgroundTeamService
    {
        private readonly IRepository repository;
        public BackgroundTeamService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<int>> GetPlayersToAssignOrRemoveFromTeamsTeamRequestIdsAsync()
        {
            var teamRequests = await repository.AllReadOnly<TeamRequest>()
                .Where(tr => tr.TeamRequestStatus == TeamRequestStatus.Accepted)
                .Include(tr => tr.Team)
                .ThenInclude(t => t.GamesAsTeamRed)
                .Include(tr => tr.Team)
                .ThenInclude(t => t.GamesAsTeamBlue)
                .ToListAsync();
            List<int> teamRequestIds = new List<int>();
            foreach (var teamRequest in teamRequests)
            {
                if (teamRequest.Team.GamesAsTeamRed.All(g => g.GameStatus != GameStatus.Upcoming) && teamRequest.Team.GamesAsTeamBlue.All(g => g.GameStatus != GameStatus.Upcoming))
                    teamRequestIds.Add(teamRequest.Id);
            }
            return teamRequestIds;
        }

        public async Task AssignOrRemovePlayerFromTeamByTeamRequestIdsAsync(IEnumerable<int> ids)
        {
            var teamRequests = await repository.All<TeamRequest>()
                .Where(tr => ids.Contains(tr.Id))
                .Include(tr => tr.Player)
                .Include(tr => tr.Team)
                .ThenInclude(tr => tr.Players)
                .ToListAsync();
            foreach (var teamRequest in teamRequests)
            {
                switch (teamRequest.TeamRequestType)
                {
                    case TeamRequestType.Join:
                        teamRequest.Team.Players.Add(teamRequest.Player);
                        break;
                    case TeamRequestType.Leave:
                        teamRequest.Team.Players.Remove(teamRequest.Player);
                        break;
                }
            }
            //await repository.SaveChangesAsync();
            repository.DeleteRange<TeamRequest>(teamRequests);
            await repository.SaveChangesAsync();

        }
    }
}

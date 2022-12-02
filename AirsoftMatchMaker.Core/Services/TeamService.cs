using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly IRepository repository;
        public TeamService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> DoesPlayerHaveTeam(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();
            return player.TeamId != null;
        }

        public async Task<IEnumerable<TeamListModel>> GetAllTeamsAsync()
        {
            var teams = await repository.AllReadOnly<Team>()
                .Include(t => t.Players)
                .Select(t => new TeamListModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Wins = t.Wins,
                    Losses = t.Losses,
                    PlayersCount = t.Players.Count(p => p.IsActive),
                    AverageSkillPoints = t.Players.Count != 0 ? (int)(t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints)) : 0,
                })
                .ToListAsync();
            return teams;
        }

        public async Task<TeamViewModel> GetTeamByIdAsync(int id)
        {
            var team = await repository.AllReadOnly<Team>()
                .Where(t => t.Id == id)
                .Include(t => t.Players)
                .ThenInclude(t => t.PlayerClass)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .Select(t => new TeamViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Wins = t.Wins,
                    Losses = t.Losses,
                    AverageSkillPoints = t.Players.Count != 0 ? (int)(t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints)) : 0,
                    Players = t.Players.Where((p) => p.IsActive)
                    .Select(p => new PlayerMinModel()
                    {
                        Id = p.Id,
                        UserName = p.User.UserName,
                        SkillLevel = p.SkillLevel,
                        PlayerClassName = p.PlayerClass.Name
                    })
                    .ToList(),
                    Games = t.GamesAsTeamRed.Union(t.GamesAsTeamBlue).Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Date = g.Date.ToShortDateString(),
                        Result = g.Result != null ? g.Result : "0:0"
                    })
                    .ToHashSet()
                })
                .FirstOrDefaultAsync();
            return team;
        }

        public async Task<TeamViewModel> GetPlayersTeamAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();
 
            var team = await repository.AllReadOnly<Team>()
                .Where(t => t.Players.Contains(player))
                .Include(t => t.Players)
                .ThenInclude(t => t.PlayerClass)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .Select(t => new TeamViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Wins = t.Wins,
                    Losses = t.Losses,
                    AverageSkillPoints = t.Players.Count != 0 ? (int)(t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints)) : 0,
                    Players = t.Players.Where((p) => p.IsActive)
                    .Select(p => new PlayerMinModel()
                    {
                        Id = p.Id,
                        UserName = p.User.UserName,
                        SkillLevel = p.SkillLevel,
                        PlayerClassName = p.PlayerClass.Name
                    })
                    .ToList(),
                    Games = t.GamesAsTeamRed.Union(t.GamesAsTeamBlue).Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Date = g.Date.ToShortDateString(),
                        Result = g.Result != null ? g.Result : "0:0"
                    })
                    .ToHashSet()
                })
                .FirstOrDefaultAsync();
            return team;
        }


    }
}

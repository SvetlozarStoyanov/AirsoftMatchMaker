using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Xml.Linq;

namespace AirsoftMatchMaker.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly IRepository repository;
        public TeamService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> TeamExistsAsync(int id)
        {
            return await repository.GetByIdAsync<Team>(id) != null;
        }

        public async Task<bool> DoesUserHaveTeamAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Include(p => p.TeamRequests)
                .FirstOrDefaultAsync();

            return player.TeamId != null;
        }

        public async Task<bool> DoesTeamWithSameNameExistAsync(string name)
        {
            return await repository.AllReadOnly<Team>()
                .AnyAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<TeamsQueryModel> GetAllTeamsAsync(
            string? searchTerm = null,
            TeamSorting sorting = TeamSorting.Newest,
            int teamsPerPage = 6,
            int currentPage = 1
            )
        {
            var teams = await repository.AllReadOnly<Team>()
                .Include(t => t.Players)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teams = teams.Where(t => t.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (sorting)
            {
                case TeamSorting.Newest:
                    teams = teams.OrderByDescending(t => t.Id).ToList();
                    break;
                case TeamSorting.Oldest:
                    teams = teams.OrderBy(t => t.Id).ToList();
                    break;
                case TeamSorting.PlayerCountAscending:
                    teams = teams.OrderBy(t => t.Players.Count).ToList();
                    break;
                case TeamSorting.PlayerCountDescending:
                    teams = teams.OrderByDescending(t => t.Players.Count).ToList();

                    break;
                case TeamSorting.Wins:
                    teams = teams.OrderByDescending(t => t.Wins).ToList();
                    break;
                default:
                    break;
            }
            var filteredTeams = teams
                .Skip((currentPage - 1) * teamsPerPage)
                .Take(teamsPerPage)
                .Select(t => new TeamListModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Wins = t.Wins,
                    Losses = t.Losses,
                    PlayersCount = t.Players.Count(p => p.IsActive),
                    AverageSkillPoints = t.Players.Count != 0 ? (int)(t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints)) : 0,
                })
            .ToList();
            TeamsQueryModel model = CreateTeamsQueryModel();
            model.TeamsCount = teams.Count;
            model.Teams = filteredTeams;
            return model;
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
                    AverageSkillLevel = t.Players.Count != 0 ? DetermineAverageSkillLevel((t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints))) : SkillLevel.Beginner,
                    Players = t.Players.Where((p) => p.IsActive)
                    .Select(p => new PlayerMinModel()
                    {
                        Id = p.Id,
                        UserName = p.User.UserName,
                        SkillLevel = p.SkillLevel,
                        PlayerClassName = p.PlayerClass.Name
                    })
                    .ToList(),
                    Games = t.GamesAsTeamRed.Union(t.GamesAsTeamBlue)
                    .Where(g => g.GameStatus == GameStatus.Finished)
                    .OrderByDescending(g => g.Date)
                    .Take(6)
                    .Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",

                        Date = g.Date.ToShortDateString(),
                        Result = g.Result != null ? g.Result : "Not played yet!"
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
                    AverageSkillLevel = t.Players.Count != 0 ? DetermineAverageSkillLevel((t.Players.Where(p => p.IsActive).Average(p => p.SkillPoints))) : SkillLevel.Beginner,
                    Players = t.Players.Where(p => p.IsActive)
                    .Select(p => new PlayerMinModel()
                    {
                        Id = p.Id,
                        UserName = p.User.UserName,
                        SkillLevel = p.SkillLevel,
                        PlayerClassName = p.PlayerClass.Name
                    })
                    .ToList(),
                    Games = t.GamesAsTeamRed.Union(t.GamesAsTeamBlue)
                    .Where(g => g.GameStatus == GameStatus.Finished)
                    .OrderByDescending(g => g.Date)
                    .Take(6)
                    .Select(g => new GameMinModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",
                        Date = g.Date.ToShortDateString(),
                        Result = g.Result != null ? g.Result : "Not played yet!"
                    })
                    .ToHashSet()
                })
                .FirstOrDefaultAsync();
            return team;
        }

        public async Task CreateTeamAsync(string userId, TeamCreateModel model)
        {
            var player = await repository.All<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            var team = new Team()
            {
                Name = model.Name,
            };
            await repository.AddAsync<Team>(team);
            team.Players.Add(player);
            await repository.SaveChangesAsync();
        }


        private TeamsQueryModel CreateTeamsQueryModel()
        {
            var model = new TeamsQueryModel();
            model.SortingOptions = Enum.GetValues<TeamSorting>().ToList();
            return model;
        }

        private static SkillLevel DetermineAverageSkillLevel(double skillPoints)
        {
            SkillLevel skillLevel = SkillLevel.Beginner;
            switch (skillPoints)
            {
                case > 800:
                    skillLevel = SkillLevel.Expert;
                    break;
                case > 600:
                    skillLevel = SkillLevel.Skilled;
                    break;
                case > 300:
                    skillLevel = SkillLevel.Intermediate;
                    break;
            }
            return skillLevel;
        }
    }
}

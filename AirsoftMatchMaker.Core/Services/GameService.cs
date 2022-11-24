using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;
        public GameService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<IEnumerable<GameListModel>> GetAllGamesAsync()
        {
            var games = await repository.AllReadOnly<Game>()
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .Include(g => g.Matchmaker)
                .ThenInclude(mm => mm.User)
                .Select(g => new GameListModel()
                {
                    Id = g.Id,
                    Name = $"{g.TeamRed.Name} VS {g.TeamBlue.Name}",
                    GameStatus = g.GameStatus,
                    Date = g.Date.ToShortDateString(),
                    EntryFee = g.EntryFee,
                    GameModeId = g.GameModeId,
                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MatchmakerId = g.MatchmakerId,
                    MatchmakerName = g.Matchmaker.User.UserName,
                    Result = g.Result != null ? g.Result : $"{g.TeamRedPoints}:{g.TeamBluePoints}",
                })
                .ToListAsync();
            return games;
        }

        public async Task<GameViewModel> GetGameByIdAsync(int id)
        {
            var game = await repository.AllReadOnly<Game>()
                        .Where(g => g.Id == id)
                        .Include(g => g.TeamRed)
                        .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                        .Include(g => g.TeamBlue)
                        .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                        .Include(g => g.Matchmaker)

                        .ThenInclude(mm => mm.User)
                        .Select(g => new GameViewModel()
                        {
                            Id = g.Id,
                            Name = $"{g.TeamRed.Name} VS {g.TeamBlue.Name}",
                            GameStatus = g.GameStatus,
                            Date = g.Date.ToShortDateString(),
                            EntryFee = g.EntryFee,
                            GameModeId = g.GameModeId,
                            GameModeName = g.GameMode.Name,
                            MapId = g.MapId,
                            MapName = g.Map.Name,
                            MatchmakerId = g.MatchmakerId,
                            MatchmakerName = g.Matchmaker.User.UserName,
                            TeamRedId = g.TeamRedId,
                            TeamRedName = g.TeamRed.Name,
                            TeamRedBets = g.Bets.Count(b => b.WinningTeamId == g.TeamRedId),
                            TeamRedOdds = g.TeamRedOdds,
                            TeamRedPlayers = g.TeamRed.Players
                            .Select(p => new PlayerMinModel()
                            {
                                Id = p.Id,
                                UserName = p.User.UserName,
                                SkillLevel = p.SkillLevel
                            })
                            .ToList(),
                            TeamBlueId = g.TeamBlueId,
                            TeamBlueName = g.TeamBlue.Name,
                            TeamBlueBets = g.Bets.Count(b => b.WinningTeamId == g.TeamBlueId),
                            TeamBlueOdds = g.TeamBlueOdds,
                            TeamBluePlayers = g.TeamBlue.Players
                            .Select(p => new PlayerMinModel()
                            {
                                Id = p.Id,
                                UserName = p.User.UserName,
                                SkillLevel = p.SkillLevel
                            })
                            .ToList(),

                            Result = g.Result != null ? g.Result : $"{g.TeamRedPoints}:{g.TeamBluePoints}",

                        })
                        .FirstOrDefaultAsync();
            return game;
        }
        public async Task<GameCreateModel> CreateGameModel(string dateTimeString)
        {
            DateTime dateTime = DateTime.Parse(dateTimeString);
            var maps = await repository.AllReadOnly<Map>()
                .Include(m => m.GameMode)
                .ToListAsync();
            foreach (var map in maps)
            {
                map.Games = map.Games.Where(g => g.GameStatus == GameStatus.Upcoming && g.Date == dateTime.Date).ToList();
            }
            maps = maps.Where(m => m.Games.Count == 0).ToList();

            var teams = await repository.AllReadOnly<Team>()
                .Where(t => t.Players.Count > 0)
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();
            foreach (var team in teams)
            {
                team.GamesAsTeamRed = team.GamesAsTeamRed.Where(g => g.Date == dateTime.Date).ToList();
                team.GamesAsTeamBlue = team.GamesAsTeamBlue.Where(g => g.Date == dateTime.Date).ToList();
            }
            teams = teams.Where(t => t.GamesAsTeamRed.Count == 0 && t.GamesAsTeamBlue.Count == 0).ToList();
            GameCreateModel model = new GameCreateModel()
            {
                DateString = dateTimeString,
                Maps = maps.Select(m => new MapSelectModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    GameModeId = m.GameModeId,
                    GameModeName = m.GameMode.Name
                }).ToList(),
                Teams = teams.Select(t => new TeamSelectModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    AverageSkillPoints = t.Players.Average(p => p.SkillPoints)
                }).ToList()
            };
            return model;
        }
        public async Task CreateGameAsync(string matchmakerUserId, GameCreateModel model)
        {
            var matchmaker = await repository.All<Matchmaker>()
                .FirstOrDefaultAsync(mm => mm.UserId == matchmakerUserId);
            var teamRed = await repository.All<Team>()
                .Where(t => t.Id == model.TeamRedId)
                .Include(t => t.Players.Where(p => p.IsActive))
                .ThenInclude(t => t.Weapons)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .FirstOrDefaultAsync();

            var teamBlue = await repository.All<Team>()
              .Where(t => t.Id == model.TeamBlueId)
              .Include(t => t.Players.Where(p => p.IsActive))
              .ThenInclude(t => t.Weapons)
              .Include(t => t.GamesAsTeamRed)
              .Include(t => t.GamesAsTeamBlue)
              .FirstOrDefaultAsync();

            DateTime dateTime = DateTime.Parse(model.DateString);

            if (teamRed.GamesAsTeamRed.Any(g => g.Date.Date == dateTime.Date) || teamRed.GamesAsTeamBlue.Any(g => g.Date.Date == dateTime.Date))
            {
                return;
            }
            if (teamBlue.GamesAsTeamRed.Any(g => g.Date.Date == dateTime.Date) || teamBlue.GamesAsTeamBlue.Any(g => g.Date.Date == dateTime.Date))
            {
                return;
            }

            var map = await repository.All<Map>()
                .Where(m => m.Id == model.MapId)
                .Include(m => m.GameMode)
                .FirstOrDefaultAsync();

            var teamRedAverageSkillPoints = teamRed.Players.Average(p => p.SkillPoints);
            var teamBlueAverageSkillPoints = teamBlue.Players.Average(p => p.SkillPoints);

            var teamRedAppropriateEngagementRangePlayers = 0;
            foreach (var player in teamRed.Players)
            {
                foreach (var weapon in player.Weapons)
                {
                    if (weapon.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString())
                    {
                        teamRedAppropriateEngagementRangePlayers++;
                        break;
                    }
                }
            }
            var teamBlueAppropriateEngagementRangePlayers = 0;
            foreach (var player in teamBlue.Players)
            {
                foreach (var weapon in player.Weapons)
                {
                    if (weapon.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString())
                    {
                        teamBlueAppropriateEngagementRangePlayers++;
                        break;
                    }
                }
            }
            var teamRedImpact = CalculateTeamImpact(teamRedAverageSkillPoints, teamRedAppropriateEngagementRangePlayers, teamRed.Players.Count(p => p.IsActive), teamRed.Wins, teamRed.Losses);
            var teamBlueImpact = CalculateTeamImpact(teamBlueAverageSkillPoints, teamBlueAppropriateEngagementRangePlayers, teamBlue.Players.Count(p => p.IsActive), teamBlue.Wins, teamBlue.Losses);

            (int teamRedOdds, int teamBlueOdds) odds = CalculateOdds(teamRedImpact, teamBlueImpact);

            Game game = new Game()
            {
                Name = $"{teamRed.Name} VS {teamBlue.Name} {map.Name} {dateTime.Date}",
                EntryFee = model.EntryFee,
                GameStatus = GameStatus.Upcoming,
                Date = dateTime,
                Result = null,
                MatchmakerId = matchmaker.Id,
                MapId = model.MapId,
                GameModeId = map.GameModeId,
                TeamRedId = model.TeamRedId,
                TeamBlueId = model.TeamBlueId,
                TeamRedOdds = odds.teamRedOdds,
                TeamBlueOdds = odds.teamBlueOdds
            };
            matchmaker.OrganisedGames.Add(game);
            //await repository.AddAsync<Game>(game);
            await repository.SaveChangesAsync();
        }



        private int CalculateTeamImpact(double averageSkillPoints, int appropriateEngagementRangePlayers, int playerCount, int wins, int losses)
        {
            var teamImpact = averageSkillPoints;
            if (appropriateEngagementRangePlayers == 0)
            {
                teamImpact *= 0.90;
            }
            else
            {
                teamImpact *= 1 + ((double)appropriateEngagementRangePlayers) / 10;
            }
            if (wins > losses)
            {
                teamImpact += (wins - losses) * 2;
            }
            else if (wins < losses)
            {
                teamImpact += (losses - wins) * 2;
            }

            return (int)Math.Round(teamImpact, 0);
        }

        private ValueTuple<int, int> CalculateOdds(int teamRedImpact, int teamBlueImpact)
        {
            int impactDifference = Math.Abs(teamRedImpact - teamBlueImpact) + 100;

            (int teamRedOdds, int teamBlueOdds) odds = new ValueTuple<int, int>();
            if (teamRedImpact > teamBlueImpact)
            {
                odds.teamRedOdds = -impactDifference;
                odds.teamBlueOdds = +impactDifference;
            }
            else if (teamRedImpact < teamBlueImpact)
            {
                odds.teamRedOdds = +impactDifference;
                odds.teamBlueOdds = -impactDifference;
            }
            else
            {
                odds.teamRedOdds = -impactDifference;
                odds.teamBlueOdds = -impactDifference;
            }
            return odds;
        }


    }
}

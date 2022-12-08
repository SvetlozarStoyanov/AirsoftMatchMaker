using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;
        public GameService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<bool> GameExistsAsync(int id)
        {
            var game = await repository.GetByIdAsync<Game>(id);
            return game != null;
        }

        public async Task<bool> AreTeamPlayerCountsSimilarAsync(int teamRedId, int teamBlueId)
        {
            var teamRed = await repository.AllReadOnly<Team>()
                .Where(t => t.Id == teamRedId)
                .Include(t => t.Players)
                .FirstOrDefaultAsync();
            var teamBlue = await repository.AllReadOnly<Team>()
               .Where(t => t.Id == teamBlueId)
               .Include(t => t.Players)
               .FirstOrDefaultAsync();
            return Math.Abs(teamRed.Players.Count - teamBlue.Players.Count) <= 2;
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
                    Name = g.Name,
                    GameStatus = g.GameStatus,
                    Date = g.Date,
                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MapImageUrl = g.Map.ImageUrl,
                    TerrainType = g.Map.Terrain,
                    TeamRedId = g.TeamRedId,
                    TeamRedName = g.TeamRed.Name,
                    TeamRedOdds = g.TeamRedOdds,
                    TeamBlueId = g.TeamBlueId,
                    TeamBlueName = g.TeamBlue.Name,
                    TeamBlueOdds = g.TeamBlueOdds,
                    Result = g.Result != null ? g.Result : "Not played yet",
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
                            Name = g.Name,
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
                            Result = g.Result != null ? g.Result : "Not played yet",
                        })
                        .FirstOrDefaultAsync();
            return game;
        }


        public async Task<GameSelectDateModel> GetNextSevenAvailableDatesAsync()
        {
            DateTime dateTime = DateTime.Today.AddHours(36);
            HashSet<DateTime> availableDates = new HashSet<DateTime>();

            var maps = await repository.AllReadOnly<Map>()
                 .Include(m => m.GameMode)
                 .Include(m => m.Games)
                 .ToListAsync();

            var teams = await repository.AllReadOnly<Team>()
                .Where(t => t.Players.Count > 0 && t.Players.Any(p => p.Weapons.Count > 0) && t.GamesAsTeamRed.All(g => g.GameStatus != GameStatus.Upcoming) && t.GamesAsTeamBlue.All(g => g.GameStatus != GameStatus.Upcoming))
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();
            int teamsCount = teams.Count;
            if (teams.Count < 2)
            {
                return null;
            }
            else if (teamsCount == 2 && Math.Abs(teams.First().Players.Count - teams.Last().Players.Count) > 2)
            {
                return null;
            }
            int i = 0;
            while (availableDates.Count < 7 && i < 30)
            {
                int mapCount = maps.Count;
                foreach (var map in maps)
                {
                    if (map.Games.Any(g => g.Date.Date == dateTime.Date))
                    {
                        mapCount--;
                    }
                }
                if (mapCount > 0)
                {
                    availableDates.Add(dateTime);
                }
                dateTime = dateTime.AddDays(1);
                i++;
            }
            if (availableDates.Count == 0)
            {
                return null;
            }
            var model = new GameSelectDateModel()
            {
                Dates = availableDates
                .Select(d => d.ToString())
                .ToList()
            };

            return model;
        }

        public async Task<GameCreateModel> CreateGameModelAsync(string dateTimeString)
        {
            DateTime dateTime = DateTime.Parse(dateTimeString);
            var maps = await repository.AllReadOnly<Map>()
                .Include(m => m.GameMode)
                .Include(m => m.Games)
                .ToListAsync();
            foreach (var map in maps)
            {
                map.Games = map.Games.Where(g => g.GameStatus == GameStatus.Upcoming && g.Date.Day == dateTime.Date.Day).ToList();
            }
            maps = maps.Where(m => m.Games.Count == 0).ToList();

            var teams = await repository.AllReadOnly<Team>()
                .Where(t => t.Players.Count(p => p.IsActive) > 0 && t.Players.Any(p => p.Weapons.Count > 0) && t.GamesAsTeamRed.All(g => g.GameStatus != GameStatus.Upcoming) && t.GamesAsTeamBlue.All(g => g.GameStatus != GameStatus.Upcoming))
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();

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
                    Name = t.Players.Count > 1 ? $"{t.Name} ({t.Players.Count} players)" : $"{t.Name} ({t.Players.Count} player)",
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
                .Include(t => t.Players.Where(p => p.IsActive && p.Weapons.Any()))
                .ThenInclude(t => t.Weapons)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .FirstOrDefaultAsync();

            var teamBlue = await repository.All<Team>()
              .Where(t => t.Id == model.TeamBlueId)
              .Include(t => t.Players.Where(p => p.IsActive && p.Weapons.Any()))
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

            var teamRedImpact = CalculateTeamImpactForBettingOdds(teamRed, map.AverageEngagementDistance.ToString());
            var teamBlueImpact = CalculateTeamImpactForBettingOdds(teamBlue, map.AverageEngagementDistance.ToString());




            (int teamRedOdds, int teamBlueOdds) odds = CalculateOdds(teamRedImpact, teamBlueImpact);

            Game game = new Game()
            {
                Name = $"{teamRed.Name} VS {teamBlue.Name}",
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
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameListModel>> GetUpcomingGamesByDateAsync()
        {
            var games = await repository.AllReadOnly<Game>()
               .Where(g => g.GameStatus != GameStatus.Finished)
               .Include(g => g.TeamRed)
               .Include(g => g.TeamBlue)
               .Include(g => g.Matchmaker)
               .ThenInclude(mm => mm.User)
               .Include(g => g.Map)
               .Select(g => new GameListModel()
               {
                   Id = g.Id,
                   Name = g.Name,
                   GameStatus = g.GameStatus,
                   Date = g.Date,
                   GameModeName = g.GameMode.Name,
                   MapId = g.MapId,
                   MapName = g.Map.Name,
                   MapImageUrl = g.Map.ImageUrl,
                   TerrainType = g.Map.Terrain,
                   TeamRedId = g.TeamRedId,
                   TeamRedName = g.TeamRed.Name,
                   TeamRedOdds = g.TeamRedOdds,
                   TeamBlueId = g.TeamBlueId,
                   TeamBlueName = g.TeamBlue.Name,
                   TeamBlueOdds = g.TeamBlueOdds
               })
               .ToListAsync();
            return games;

        }

        private int CalculateTeamImpactForBettingOdds(Team team, string averageEngagementDistance)
        {
            var averageSkillPoints = team.Players.Average(p => p.SkillPoints);
            var appropriateEngagementRangePlayers = 0;
            foreach (var player in team.Players)
            {
                if (player.Weapons.Any(w => w.PreferedEngagementDistance.ToString() == averageEngagementDistance))
                    appropriateEngagementRangePlayers++;
            }
            var teamImpact = averageSkillPoints;
            if (appropriateEngagementRangePlayers == 0)
            {
                teamImpact *= 0.90;
            }
            else
            {
                teamImpact *= 1 + ((double)appropriateEngagementRangePlayers) / 10;
            }
            if (team.Wins > team.Losses)
            {
                teamImpact += (team.Wins - team.Losses) * 2;
            }
            else if (team.Wins < team.Losses)
            {
                teamImpact += (team.Losses - team.Wins) * 2;
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

        public async Task<IEnumerable<GameListModel?>> GetPlayersLastFinishedAndFirstUpcomingGameAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Include(p => p.Team)
                .ThenInclude(t => t.GamesAsTeamRed)
                .Include(p => p.Team)
                .ThenInclude(t => t.GamesAsTeamBlue)
                .FirstOrDefaultAsync();
            if (player.TeamId == null)
            {
                return null;
            }
            var games = player.Team.GamesAsTeamRed.Union(player.Team.GamesAsTeamBlue).ToList();
            if (games.Count == 0)
            {
                return null;
            }
            var lastGameId = games.OrderByDescending(g => g.Date)
                .FirstOrDefault(g => g.GameStatus == GameStatus.Finished).Id;
            var lastGame = new GameListModel();
            if (lastGameId != null)
            {
                lastGame = await repository.AllReadOnly<Game>()
                    .Where(g => g.Id == lastGameId)
                    .Select(g => new GameListModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Date = g.Date,
                        GameModeName = g.GameMode.Name,
                        MapId = g.MapId,
                        MapName = g.Map.Name,
                        MapImageUrl = g.Map.ImageUrl,
                        TerrainType = g.Map.Terrain,
                        TeamRedId = g.TeamRedId,
                        TeamRedName = g.TeamRed.Name,
                        TeamRedOdds = g.TeamRedOdds,
                        TeamBlueId = g.TeamBlueId,
                        TeamBlueName = g.TeamBlue.Name,
                        TeamBlueOdds = g.TeamBlueOdds,
                        Result = g.Result != null ? g.Result : "Not played yet",
                    })
                    .FirstOrDefaultAsync();
            }
            else
            {
                lastGame = null;
            }
            var nextGameId = games.OrderBy(g => g.Date)
                .FirstOrDefault(g => g.GameStatus == GameStatus.Upcoming).Id;
            var nextGame = new GameListModel();
            if (lastGameId != null)
            {
                nextGame = await repository.AllReadOnly<Game>()
                    .Where(g => g.Id == nextGameId)
                    .Include(g => g.Map)
                    .Include(g => g.GameMode)
                    .Select(g => new GameListModel()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        GameStatus = g.GameStatus,
                        Date = g.Date,
                        GameModeName = g.GameMode.Name,
                        MapId = g.MapId,
                        MapName = g.Map.Name,
                        MapImageUrl = g.Map.ImageUrl,
                        TerrainType = g.Map.Terrain,
                        TeamRedId = g.TeamRedId,
                        TeamRedName = g.TeamRed.Name,
                        TeamRedOdds = g.TeamRedOdds,
                        TeamBlueId = g.TeamBlueId,
                        TeamBlueName = g.TeamBlue.Name,
                        TeamBlueOdds = g.TeamBlueOdds,
                        Result = g.Result != null ? g.Result : "Not played yet",
                    })
                   .FirstOrDefaultAsync();
            }
            else
            {
                nextGame = null;
            }
            var playerGames = new List<GameListModel?>()
            {
                lastGame,nextGame
            };
            return playerGames;
        }
    }
}

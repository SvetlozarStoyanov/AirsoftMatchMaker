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
                    //mapCount -= map.Games.Count(g => g.Date.Day == dateTime.Date.Day);
                }
                //foreach (var team in teams)
                //{
                //    if (team.GamesAsTeamRed.Any(g => g.GameStatus == GameStatus.Upcoming) || team.GamesAsTeamBlue.Any(g => g.GameStatus == GameStatus.Upcoming))
                //    {
                //        teamsCount--;
                //    }
                //}
                if (/*teamsCount > 1 &&*/ mapCount > 0)
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
                .Where(t => t.Players.Count > 0 && t.Players.Any(p => p.Weapons.Count > 0) && t.GamesAsTeamRed.All(g => g.GameStatus != GameStatus.Upcoming) && t.GamesAsTeamBlue.All(g => g.GameStatus != GameStatus.Upcoming))
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();
            //foreach (var team in teams)
            //{
            //    team.GamesAsTeamRed = team.GamesAsTeamRed.Where(g => g.GameStatus == GameStatus.Upcoming).ToList();
            //    team.GamesAsTeamBlue = team.GamesAsTeamBlue.Where(g => g.GameStatus == GameStatus.Upcoming).ToList();
            //}
            //teams = teams.Where(t => t.GamesAsTeamRed.Count == 0 && t.GamesAsTeamBlue.Count == 0).ToList();
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
            await repository.SaveChangesAsync();
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


    }
}

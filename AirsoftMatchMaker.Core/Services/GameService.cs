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
using System.Runtime.CompilerServices;

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
            DateTime dateTime = DateTime.Today.AddHours(12);
            HashSet<DateTime> availableDates = new HashSet<DateTime>();

            var maps = await repository.AllReadOnly<Map>()
                 .Include(m => m.GameMode)
                 .Include(m => m.Games)
                 .ToListAsync();

            var teams = await repository.AllReadOnly<Team>()
                .Where(t => t.Players.Count > 0)
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();

            while (availableDates.Count < 7)
            {
                int mapCount = maps.Count;
                int teamsCount = teams.Count;
                foreach (var map in maps)
                {
                    mapCount -= map.Games.Count(g => g.Date.Day == dateTime.Date.Day);
                }
                foreach (var team in teams)
                {
                    teamsCount -= team.GamesAsTeamRed.Count(g => g.Date.Day == dateTime.Date.Day);
                    teamsCount -= team.GamesAsTeamBlue.Count(g => g.Date.Day == dateTime.Date.Day);
                }
                if (teamsCount > 1 && mapCount > 0)
                {
                    availableDates.Add(dateTime);
                }
                dateTime = dateTime.AddDays(1);
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
                .Where(t => t.Players.Count > 0)
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();
            foreach (var team in teams)
            {
                team.GamesAsTeamRed = team.GamesAsTeamRed.Where(g => g.Date.Day == dateTime.Date.Day).ToList();
                team.GamesAsTeamBlue = team.GamesAsTeamBlue.Where(g => g.Date.Day == dateTime.Date.Day).ToList();
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
            //await repository.AddAsync<Game>(game);
            await repository.SaveChangesAsync();
        }





        public async Task SimulateGameAsync(int gameId)
        {
            var game = await repository.All<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.Matchmaker)
                .ThenInclude(mm => mm.User)
                .Include(g => g.Map)
                .ThenInclude(m => m.GameMode)
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .FirstOrDefaultAsync();

            var teamRed = game.TeamRed;

            var teamRedPlayers = await repository.All<Player>()
                .Where(p => p.TeamId.HasValue && p.TeamId == teamRed.Id && p.IsActive)
                .Include(p => p.User)
                .Include(p => p.Clothes)
                .Include(p => p.Weapons)
                .Include(p => p.PlayerClass)
                .ToListAsync();

            var teamBlue = game.TeamBlue;

            var teamBluePlayers = await repository.All<Player>()
            .Where(p => p.TeamId.HasValue && p.TeamId == teamBlue.Id && p.IsActive)
            .Include(p => p.User)
            .Include(p => p.Clothes)
            .Include(p => p.Weapons)
            .Include(p => p.PlayerClass)
            .ToListAsync();

            var map = game.Map;

            var gameMode = map.GameMode;

            var matchmaker = game.Matchmaker;

            var teamRedImpact = CalculateTeamImpactForSimulation(teamRedPlayers, teamRed.Wins, teamRed.Losses, map);
            var teamBlueImpact = CalculateTeamImpactForSimulation(teamBluePlayers, teamBlue.Wins, teamBlue.Losses, map);

            if (Math.Abs(teamRedImpact - teamRedImpact) < 50)
            {
                teamRedImpact = AdditionalCalculationsForSimulation(teamRedPlayers, teamRedImpact, map);
                teamBlueImpact = AdditionalCalculationsForSimulation(teamBluePlayers, teamBlueImpact, map);
            }

            if (teamRedImpact == teamBlueImpact)
            {
                var random = new Random();
                var winner = random.Next(0, 1);
                if (winner == 0)
                    teamRedImpact += 1;
                else
                    teamBlueImpact += 1;
            }

            if (teamRedImpact > teamBlueImpact)
            {
                AwardWinningTeam(teamRed, teamRedPlayers, map);
                AwardLosingTeam(teamBlue, teamBluePlayers, map);
            }
            else if (teamRedImpact < teamBlueImpact)
            {
                AwardLosingTeam(teamRed, teamRedPlayers, map);
                AwardWinningTeam(teamBlue, teamBluePlayers, map);
            }
            string finalScore = DetermineFinalResult(teamRedImpact, teamBlueImpact, gameMode);
            int totalPlayerCount = teamRedPlayers.Count + teamBluePlayers.Count;
            TakeEntryFeeFromPlayers(teamRedPlayers, teamBluePlayers, game.EntryFee);
            FinalizeGame(totalPlayerCount, finalScore, matchmaker, game);
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

        private int CalculateTeamImpactForSimulation(List<Player> players, int wins, int losses, Map map)
        {
            var averageSkillPoints = players.Average(p => p.SkillPoints);
            var appropriateEngagementRangePlayerCount = 0;
            var appropriateClothesPlayerCount = 0;
            foreach (var player in players)
            {
                if (player.Weapons.Any(w => w.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString()))
                    appropriateEngagementRangePlayerCount++;
                switch (map.Terrain)
                {
                    case TerrainType.Urban:
                        if (player.Clothes.Any(c => c.ClothingColor == ClothingColor.Gray))
                            appropriateClothesPlayerCount++;
                        break;
                    case TerrainType.Forest:
                        if (player.Clothes.Any(c => c.ClothingColor == ClothingColor.Green))
                            appropriateClothesPlayerCount++;
                        break;
                    case TerrainType.Field:
                        if (player.Clothes.Any(c => c.ClothingColor == ClothingColor.Brown))
                            appropriateClothesPlayerCount++;
                        break;
                    case TerrainType.Snowy:
                        if (player.Clothes.Any(c => c.ClothingColor == ClothingColor.White))
                            appropriateClothesPlayerCount++;
                        break;
                }

            }
            var teamImpact = averageSkillPoints;
            if (appropriateEngagementRangePlayerCount == 0)
            {
                teamImpact *= 0.90;
            }
            else
            {
                teamImpact *= 1 + ((double)appropriateEngagementRangePlayerCount) / 10;
            }
            if (appropriateClothesPlayerCount == 0)
            {
                teamImpact *= 0.85;
            }
            else
            {
                teamImpact *= 1 + ((double)appropriateClothesPlayerCount) / 8;
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

        private int AdditionalCalculationsForSimulation(List<Player> players, int teamImpact, Map map)
        {
            double playerImpactSum = 0;
            foreach (var player in players)
            {
                double playerImpact = 1;
                switch (player.PlayerClass.Name)
                {
                    case "New Player":
                        playerImpact = 0.8;
                        break;
                    case "Leader":
                        playerImpact = 1.8;
                        break;
                    case "Frontline":
                        playerImpact = 1.2;
                        break;
                    case "Marksman":
                        if (map.AverageEngagementDistance == AverageEngagementDistance.Long)
                        {
                            playerImpact = 1.4;
                        }
                        else if (map.AverageEngagementDistance == AverageEngagementDistance.Short)
                        {
                            playerImpact = 0.9;
                        }
                        break;
                    case "Sneaky":
                        if (map.AverageEngagementDistance == AverageEngagementDistance.Medium)
                        {
                            playerImpact = 1.3;
                        }
                        else if (map.AverageEngagementDistance == AverageEngagementDistance.Long)
                        {
                            playerImpact = 1.4;
                        }
                        break;
                    case "Camper":
                        if (map.AverageEngagementDistance == AverageEngagementDistance.Long)
                        {
                            playerImpact = 1.1;
                        }
                        else if (map.AverageEngagementDistance == AverageEngagementDistance.Medium)
                        {
                            playerImpact = 1.2;
                        }
                        break;
                    case "Rusher":
                        if (map.AverageEngagementDistance == AverageEngagementDistance.Short)
                        {
                            playerImpact = 1.3;
                        }
                        else if (map.AverageEngagementDistance == AverageEngagementDistance.Medium)
                        {
                            playerImpact = 1.2;
                        }
                        else
                        {
                            playerImpact = 0.9;
                        }
                        break;
                }
                playerImpactSum += playerImpact;
            }
            double averagePlayerImpact = playerImpactSum / players.Count;
            teamImpact = (int)(teamImpact * averagePlayerImpact);
            return teamImpact;
        }

        private void AwardWinningTeam(Team team, List<Player> players, Map map)
        {
            team.Wins += 1;
            foreach (var player in players)
            {
                var weaponUsed = DetermineWeaponUsed(player, map);
                var random = new Random();

                player.Ammo -= weaponUsed.AverageAmmoExpendedPerGame + random.Next(-30, +30);

                if (player.Ammo < 0)
                    player.Ammo = 0;

                if (player.SkillPoints < 1000)
                {
                    player.SkillPoints += 50;
                    switch (player.SkillPoints)
                    {
                        case > 800:
                            player.SkillLevel = SkillLevel.Expert;
                            break;
                        case > 600:
                            player.SkillLevel = SkillLevel.Skilled;
                            break;
                        case > 300:
                            player.SkillLevel = SkillLevel.Intermediate;
                            break;
                    }
                }
            }
        }
        private void AwardLosingTeam(Team team, List<Player> players, Map map)
        {
            team.Losses += 1;
            foreach (var player in players)
            {
                var weaponUsed = DetermineWeaponUsed(player, map);

                var random = new Random();

                player.Ammo -= weaponUsed.AverageAmmoExpendedPerGame + random.Next(-30, +30);

                if (player.Ammo < 0)
                    player.Ammo = 0;
                if (player.SkillPoints < 1000)
                {
                    player.SkillPoints += 25;
                    switch (player.SkillPoints)
                    {
                        case > 800:
                            player.SkillLevel = SkillLevel.Expert;
                            break;
                        case > 600:
                            player.SkillLevel = SkillLevel.Skilled;
                            break;
                        case > 300:
                            player.SkillLevel = SkillLevel.Intermediate;
                            break;
                    }
                }
            }
        }

        private void TakeEntryFeeFromPlayers(List<Player> teamRedPlayers, List<Player> teamBluePlayers, decimal entryFee)
        {
            foreach (var player in teamRedPlayers)
            {
                player.User.Credits -= entryFee;
            }
            foreach (var player in teamBluePlayers)
            {
                player.User.Credits -= entryFee;
            }
        }

        private string DetermineFinalResult(int teamRedImpact, int teamBlueImpact, GameMode gameMode)
        {
            int impactDifference = Math.Abs(teamRedImpact - teamBlueImpact);
            (int teamRedScore, int teamBlueScore) result = new ValueTuple<int, int>();
            if (teamRedImpact > teamBlueImpact)
            {
                result.teamRedScore = gameMode.PointsToWin;
                switch (impactDifference)
                {
                    case > 100:
                        result.teamBlueScore = gameMode.PointsToWin / 4;
                        break;
                    case > 75:
                        result.teamBlueScore = gameMode.PointsToWin / 3;
                        break;
                    case > 50:
                        result.teamBlueScore = gameMode.PointsToWin / 3;
                        break;
                    case > 25:
                        result.teamBlueScore = gameMode.PointsToWin / 2;
                        break;
                    default:
                        result.teamBlueScore = gameMode.PointsToWin - 1;
                        break;
                }
            }
            if (teamRedImpact < teamBlueImpact)
            {
                result.teamBlueScore = gameMode.PointsToWin;
                switch (impactDifference)
                {
                    case > 100:
                        result.teamRedScore = gameMode.PointsToWin / 4;
                        break;
                    case > 75:
                        result.teamRedScore = gameMode.PointsToWin / 3;
                        break;
                    case > 50:
                        result.teamRedScore = gameMode.PointsToWin / 3;
                        break;
                    case > 25:
                        result.teamRedScore = gameMode.PointsToWin / 2;
                        break;
                    default:
                        result.teamRedScore = gameMode.PointsToWin - 1;
                        break;
                }
            }
            return $"{result.teamRedScore}:{result.teamBlueScore}";
        }
        private Weapon DetermineWeaponUsed(Player player, Map map)
        {

            var weaponUsed = player.Weapons.FirstOrDefault(w => w.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString());

            if (weaponUsed == null)
            {
                var random = new Random();
                weaponUsed = player.Weapons.ToArray()[random.Next(0, player.Weapons.Count - 1)];
            }
            return weaponUsed;
        }

        private void FinalizeGame(int totalPlayersCount, string finalResult, Matchmaker matchmaker, Game game)
        {
            matchmaker.User.Credits += game.EntryFee * totalPlayersCount;
            game.Result = finalResult;
            game.GameStatus = GameStatus.Finished;
        }
    }
}

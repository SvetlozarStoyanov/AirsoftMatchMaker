using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameSimulationService : IGameSimulationService
    {
        private readonly IRepository repository;
        public GameSimulationService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<int>> FindGamesToSimulateAsync(DateTime dateTime)
        {
            var gameIds = await repository.AllReadOnly<Game>()
                .Where(g => g.Date.Date.DayOfYear < dateTime.Date.DayOfYear && g.GameStatus == GameStatus.Upcoming)
                .Select(g => g.Id)
                .ToListAsync();
            return gameIds;
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
            var finalScore = DetermineFinalResult(teamRedImpact, teamBlueImpact, gameMode);
            var finalScoreAsArray = finalScore.Split(':');
            game.TeamRedPoints = int.Parse(finalScoreAsArray[0]);
            game.TeamBluePoints = int.Parse(finalScoreAsArray[1]);
            int totalPlayerCount = teamRedPlayers.Count + teamBluePlayers.Count;
            TakeEntryFeeFromPlayers(teamRedPlayers, teamBluePlayers, game.EntryFee);
            FinalizeGame(totalPlayerCount, finalScore, matchmaker, game);
            await repository.SaveChangesAsync();
        }

        public async Task PayoutBetsByGameIdAsync(int gameId)
        {
            var game = await repository.All<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.Bets)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync();
            if (game == null || game.GameStatus == GameStatus.Upcoming)
            {
                throw new ArgumentException("Cannot payout bets on unfinished game!");
            }
            var winningTeamId = game.TeamRedPoints > game.TeamBluePoints ? game.TeamRedId : game.TeamRedPoints < game.TeamBluePoints ? game.TeamBlueId : 0;
            var winningBetters = new List<User>();
            foreach (var bet in game.Bets)
            {
                if (bet.WinningTeamId == winningTeamId)
                    bet.User.Credits += CalculateBetPayout(bet.Odds, bet.CreditsBet);
                bet.BetStatus = BetStatus.Finished;
            }
            await repository.SaveChangesAsync();
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

        private static decimal CalculateBetPayout(int odds, decimal creditsBet)
        {
            var profit = creditsBet;
            if (odds < 0)
            {
                profit += profit * (100 / (decimal)Math.Abs(odds));
            }
            else
            {
                profit += profit * ((decimal)odds / 100);
            }
            return Math.Round(profit, 2);
        }
    }
}

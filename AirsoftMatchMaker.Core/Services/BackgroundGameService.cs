using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace AirsoftMatchMaker.Core.Services
{
    public class BackgroundGameService : IBackgroundGameService
    {
        private readonly IRepository repository;

        public BackgroundGameService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task MarkGamesAsFinishedAsync(DateTime dateTime)
        {
            var games = await repository.All<Game>()
                .Where(g => g.Date.AddHours(5) < dateTime && g.GameStatus == GameStatus.Upcoming)
                .ToListAsync();
            foreach (var game in games)
            {
                game.GameStatus = GameStatus.Finished;
            }
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetGameIdsOfGamesWithNotUpDoDateOddsAsync()
        {
            var gamesIds = await repository.AllReadOnly<Game>()
                .Where(g => g.GameStatus == GameStatus.Upcoming && g.OddsAreUpdated == false)
                .Select(g => g.Id)
                .ToListAsync();
            return gamesIds;
        }

        public async Task CalculateBettingOddsAsync(int gameId)
        {
            var game = await repository.All<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.Map)
                .ThenInclude(m => m.GameMode)
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .Include(g => g.Bets)
                .Include(g => g.GameBetCreditsContainer)
                .FirstOrDefaultAsync();

            var teamRed = game.TeamRed;

            var teamRedPlayers = await repository.All<Player>()
                .Where(p => p.TeamId.HasValue && p.TeamId == teamRed.Id && p.IsActive && p.Weapons.Any() && p.User.Credits >= game.EntryFee)
                .Include(p => p.User)
                .Include(p => p.Weapons)
                .Include(p => p.PlayerClass)
                .ToListAsync();

            var teamBlue = game.TeamBlue;

            var teamBluePlayers = await repository.All<Player>()
                .Where(p => p.TeamId.HasValue && p.TeamId == teamBlue.Id && p.IsActive && p.Weapons.Any() && p.User.Credits >= game.EntryFee)
                .Include(p => p.User)
                .Include(p => p.Weapons)
                .Include(p => p.PlayerClass)
                .ToListAsync();

            var map = game.Map;

            var gameMode = map.GameMode;

            var teamRedImpact = CalculateTeamImpact(teamRedPlayers, teamRed.Wins, teamRed.Losses, map);
            var teamBlueImpact = CalculateTeamImpact(teamBluePlayers, teamBlue.Wins, teamBlue.Losses, map);

            if (Math.Abs(teamRedImpact - teamBlueImpact) < 50)
            {
                teamRedImpact = AdditionalCalculations(teamRedPlayers, teamRedImpact, map);
                teamBlueImpact = AdditionalCalculations(teamBluePlayers, teamBlueImpact, map);
            }
            (int teamRedOdds, int teamBlueOdds) odds = CalculateOdds(teamRedImpact, teamBlueImpact);
            if (game.Bets.Any())
            {
                odds = CalculateLineMovementFromBets(game.Bets.ToList(), odds, (teamRed.Id, teamBlue.Id));
            }
            game.TeamRedOdds = odds.teamRedOdds;
            game.TeamBlueOdds = odds.teamBlueOdds;
            game.OddsAreUpdated = true;
            await repository.SaveChangesAsync();
        }



        private int CalculateTeamImpact(List<Player> players, int wins, int losses, Map map)
        {
            var averageSkillPoints = players.Average(p => p.SkillPoints);
            var appropriateEngagementRangePlayerCount = 0;

            foreach (var player in players)
            {
                if (player.Weapons.Any(w => w.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString()))
                    appropriateEngagementRangePlayerCount++;
            }
            var teamImpact = averageSkillPoints;
            if (appropriateEngagementRangePlayerCount == 0)
            {
                teamImpact *= 0.90;
            }
            else
            {
                teamImpact *= 1.00 + ((double)appropriateEngagementRangePlayerCount) / 10;
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

        private int AdditionalCalculations(List<Player> players, int teamImpact, Map map)
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
                        else if (map.AverageEngagementDistance == AverageEngagementDistance.Long)
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
        private ValueTuple<int, int> CalculateLineMovementFromBets(List<Bet> bets, (int teamRedOdds, int teamBlueOdds) odds, (int teamRedId, int teamBlueId) teamIds)
        {

            foreach (var bet in bets)
            {
                if (bet.WinningTeamId == teamIds.teamRedId)
                {
                    if (odds.teamRedOdds < 0)
                    {
                        odds.teamRedOdds -= (int)Math.Round(bet.CreditsBet / 10, 0);
                        odds.teamBlueOdds += (int)Math.Round(bet.CreditsBet / 8, 0);
                        if (odds.teamBlueOdds <= -100)
                        {

                        }
                        else if (odds.teamBlueOdds < 100)
                            odds.teamBlueOdds = 200 - Math.Abs(odds.teamBlueOdds);
                    }
                    if (odds.teamRedOdds > 0)
                    {
                        odds.teamRedOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                        if (odds.teamRedOdds < 100)
                            odds.teamRedOdds = Math.Abs(odds.teamRedOdds) - 200;
                        odds.teamBlueOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                        if (odds.teamBlueOdds > -100)
                            odds.teamBlueOdds = 200 - Math.Abs(odds.teamBlueOdds);
                    }
                }
                else if (bet.WinningTeamId == teamIds.teamBlueId)
                {
                    if (odds.teamBlueOdds < 0)
                    {
                        odds.teamBlueOdds -= (int)Math.Round(bet.CreditsBet / 10, 0);

                        odds.teamRedOdds += (int)Math.Round(bet.CreditsBet / 8, 0);
                        if (odds.teamRedOdds <= -100)
                        {

                        }
                        else if (odds.teamRedOdds < 100)
                            odds.teamRedOdds = 200 - Math.Abs(odds.teamRedOdds);
                    }
                    if (odds.teamBlueOdds > 0)
                    {
                        odds.teamBlueOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                        if (odds.teamBlueOdds < 100)
                            odds.teamBlueOdds = Math.Abs(odds.teamBlueOdds) - 200;
                        odds.teamRedOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                        if (odds.teamRedOdds > -100)
                            odds.teamRedOdds = 200 - Math.Abs(odds.teamRedOdds);
                    }
                }
            }
            return odds;
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class BackgroundBetService : IBackgroundBetService
    {
        private readonly IRepository repository;

        public BackgroundBetService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<int>> GetAllFinalisedGameIdsWithUnpaidBetsAsync()
        {
            var gameIds = await repository.AllReadOnly<Game>()
                .Where(g => g.GameStatus == GameStatus.Finished && g.Result != null && !g.GameBetCreditsContainer.BetsArePaidOut)
                .Include(g => g.GameBetCreditsContainer)
                .Select(g => g.Id)
                .ToListAsync();
            return gameIds;
        }

        public async Task PayoutBetsByGameIdAsync(int gameId)
        {
            var game = await repository.All<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.Bets)
                .ThenInclude(b => b.User)
                .Include(g => g.GameBetCreditsContainer)
                .FirstOrDefaultAsync();

            var container = game.GameBetCreditsContainer;
            if (game == null || game.GameStatus == GameStatus.Upcoming)
            {
                throw new ArgumentException("Cannot payout bets on unfinished game!");
            }
            var winningTeamId = game.TeamRedPoints > game.TeamBluePoints ? game.TeamRedId : game.TeamRedPoints < game.TeamBluePoints ? game.TeamBlueId : 0;
            foreach (var bet in game.Bets)
            {
                if (bet.WinningTeamId == winningTeamId)
                {
                    decimal profit = CalculateBetProfit(bet.Odds, bet.CreditsBet);
                    if (game.TeamRedId == winningTeamId)
                    {
                        container.TeamRedCreditsBet -= bet.CreditsBet;
                        if (container.TeamBlueCreditsBet > profit)
                        {
                            container.TeamBlueCreditsBet -= profit;
                        }
                        else
                        {
                            profit = container.TeamBlueCreditsBet;
                            container.TeamBlueCreditsBet = 0;
                        }
                        bet.User.Credits += bet.CreditsBet + profit;
                    }
                    else if (game.TeamRedId == winningTeamId)
                    {
                        container.TeamRedCreditsBet -= bet.CreditsBet;
                        if (container.TeamBlueCreditsBet > profit)
                        {
                            container.TeamBlueCreditsBet -= profit;
                        }
                        else
                        {
                            profit = container.TeamBlueCreditsBet;
                            container.TeamBlueCreditsBet = 0;
                        }
                        bet.User.Credits += bet.CreditsBet + profit;
                    }
                }
                bet.BetStatus = BetStatus.Finished;
            }
            container.BetsArePaidOut = true;
            await repository.SaveChangesAsync();
        }

        private static decimal CalculateBetProfit(int odds, decimal creditsBet)
        {
            var profit = 0m;
            if (odds < 0)
            {
                profit += creditsBet * (100 / (decimal)Math.Abs(odds));
            }
            else
            {
                profit += creditsBet * ((decimal)odds / 100);
            }
            return Math.Round(profit, 2);
        }
    }
}

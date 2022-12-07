using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class BetService : IBetService
    {
        private readonly IRepository repository;

        public BetService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<bool> BetExistsAsync(int id)
        {
            var bet = await repository.GetByIdAsync<Bet>(id);
            return bet != null;
        }

        public async Task<bool> HasUserAlreadyBetOnGameAsync(string userId, int gameId)
        {
            return await repository.AllReadOnly<Bet>()
                .AnyAsync(b => b.UserId == userId && b.GameId == gameId);
        }

        public async Task<bool> IsUserInOneOfTheTeamsInTheGameAsync(string userId, int gameId)
        {
            var game = await repository.AllReadOnly<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.TeamRed)
                .ThenInclude(t => t.Players)
                .Include(g => g.TeamBlue)
                .ThenInclude(t => t.Players)
                .FirstOrDefaultAsync();
            var IsUserInTeam = game.TeamRed.Players.Any(p => p.UserId == userId) || game.TeamBlue.Players.Any(p => p.UserId == userId);
            return IsUserInTeam;
        }

        public async Task<bool> IsGameFinishedAsync(int gameId)
        {
            var game = await repository.GetByIdAsync<Game>(gameId);
            return game.GameStatus == GameStatus.Finished;
        }
        public async Task<IEnumerable<BetListModel>> GetUserBetsAsync(string userId)
        {
            var bets = await repository.AllReadOnly<Bet>()
                .Where(b => b.UserId == userId)
                .Include(b => b.Game)
                .Select(b => new BetListModel()
                {
                    Id = b.Id,
                    GameName = b.Game.Name,
                    CreditsBet = b.CreditsBet,
                    ChosenTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    Odds = b.Odds,
                    BetStatus = b.BetStatus,
                })
                .ToListAsync();
            return bets;
        }



        public async Task<BetCreateModel> CreateBetCreateModelAsync(string userId, int gameId)
        {
            var user = await repository.GetByIdAsync<User>(userId);
            var game = await repository.AllReadOnly<Game>()
                .Where(g => g.Id == gameId)
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .FirstOrDefaultAsync();
            if (user == null || game == null || game.GameStatus == GameStatus.Finished)
            {
                return null;
            }
            var model = new BetCreateModel()
            {
                UserId = userId,
                GameId = gameId,
                GameName = game.Name,
                TeamRedId = game.TeamRedId,
                TeamRedName = game.TeamRed.Name,
                TeamRedOdds = game.TeamRedOdds,
                TeamBlueId = game.TeamBlueId,
                TeamBlueName = game.TeamBlue.Name,
                TeamBlueOdds = game.TeamBlueOdds,
                UserCredits = user.Credits
            };
            return model;
        }

        public async Task CreateBetAsync(string userId, BetCreateModel model)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            var game = await repository.All<Game>()
                .Where(g => g.Id == model.GameId)
                .Include(g => g.TeamBlue)
                .ThenInclude(t => t.Players)
                .Include(g => g.TeamRed)
                .ThenInclude(t => t.Players)
                .Include(g => g.Bets)
                .FirstOrDefaultAsync();

            if (game.TeamRed.Players.Any(p => p.UserId == userId) || game.TeamBlue.Players.Any(p => p.UserId == userId))
            {
                return;
            }
            user.Credits -= model.CreditsPlaced;

            var bet = new Bet()
            {
                UserId = userId,
                GameId = model.GameId,
                Odds = game.TeamRedId == model.TeamRedId ? game.TeamRedOdds : game.TeamBlueOdds,
                CreditsBet = model.CreditsPlaced,
                WinningTeamId = model.WinningTeamId,
                BetStatus = BetStatus.Active
            };
            game.Bets.Add(bet);
            await repository.SaveChangesAsync();
            if (model.WinningTeamId == game.TeamRedId)
            {
                if (game.TeamRedOdds < 0)
                {
                    game.TeamRedOdds -= (int)Math.Round(model.CreditsPlaced / 10, 0);
                    //if (game.TeamRedOdds > -100)
                    //    game.TeamRedOdds *= -1;
                    game.TeamBlueOdds += (int)Math.Round(model.CreditsPlaced / 8, 0);
                    if (game.TeamBlueOdds <= -100)
                    {

                    }
                    else if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = 200 - Math.Abs(game.TeamBlueOdds);
                }
                if (game.TeamRedOdds > 0)
                {
                    game.TeamRedOdds -= (int)Math.Round(model.CreditsPlaced / 8, 0);
                    if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = Math.Abs(game.TeamRedOdds) - 200;
                    game.TeamBlueOdds += (int)Math.Round(model.CreditsPlaced / 10, 0);
                    if (game.TeamBlueOdds > -100)
                        game.TeamBlueOdds = 200 - Math.Abs(game.TeamBlueOdds);
                }
            }
            else if (model.WinningTeamId == game.TeamBlueId)
            {
                if (game.TeamBlueOdds < 0)
                {
                    game.TeamBlueOdds -= (int)Math.Round(model.CreditsPlaced / 10, 0);

                    game.TeamRedOdds += (int)Math.Round(model.CreditsPlaced / 8, 0);
                    if (game.TeamRedOdds <= -100)
                    {

                    }
                    else if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = 200 - Math.Abs(game.TeamRedOdds);
                }
                if (game.TeamBlueOdds > 0)
                {
                    game.TeamBlueOdds -= (int)Math.Round(model.CreditsPlaced / 8, 0);
                    if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = Math.Abs(game.TeamBlueOdds) - 200;
                    game.TeamRedOdds += (int)Math.Round(model.CreditsPlaced / 10, 0);
                    if (game.TeamRedOdds > -100)
                        game.TeamRedOdds = 200 - Math.Abs(game.TeamRedOdds);
                }
            }
            await repository.SaveChangesAsync();
        }

        public async Task<BetViewModel> GetBetByIdAsync(int id)
        {
            var bet = await repository.AllReadOnly<Bet>()
                .Where(b => b.Id == id)
                .Select(b => new BetViewModel()
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GameId = b.GameId,
                    BetStatus = b.BetStatus,
                    GameName = b.Game.Name,
                    WinningTeamId = b.WinningTeamId,
                    WinningTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    CreditsBet = b.CreditsBet,
                    Odds = b.Odds,
                    PotentialProfit = CalculateProfit(b.Odds, b.CreditsBet),
                })
                .FirstOrDefaultAsync();
            return bet;
        }

        public async Task<BetDeleteModel> GetBetToDeleteByIdAsync(int id)
        {
            var bet = await repository.AllReadOnly<Bet>()
                .Where(b => b.Id == id)
                .Include(b => b.User)
                .Include(b => b.Game)
                .Select(b => new BetDeleteModel()
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GameName = b.Game.Name,
                    CreditsBet = b.CreditsBet,
                    WinningTeamId = b.WinningTeamId,
                    WinningTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    PotentialProfit = CalculateProfit(b.Odds, b.CreditsBet),
                    Odds = b.Odds
                })
                .FirstOrDefaultAsync();
            return bet;
        }

        public async Task DeleteBetAsync(BetDeleteModel model)
        {
            var bet = await repository.All<Bet>()
                .Where(b => b.Id == model.Id)
                .Include(b => b.Game)
                .Include(b => b.User)
                .FirstOrDefaultAsync();

            var user = bet.User;
            user.Credits += bet.CreditsBet;
            var game = bet.Game;
            if (bet.WinningTeamId == game.TeamRedId)
            {
                if (game.TeamRedOdds < 0)
                {
                    game.TeamRedOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                    if (game.TeamRedOdds <= -100)
                    {

                    }
                    else if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = 200 - Math.Abs(game.TeamRedOdds);
                    game.TeamBlueOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                    if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = Math.Abs(game.TeamBlueOdds) - 200;
                }
                if (game.TeamRedOdds > 0)
                {
                    game.TeamRedOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                    if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = Math.Abs(game.TeamRedOdds) - 200;
                    game.TeamBlueOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                    if (game.TeamBlueOdds <= -100)
                    {

                    }
                    else if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = 200 - Math.Abs(game.TeamBlueOdds);
                }
            }
            else if (bet.WinningTeamId == game.TeamBlueId)
            {
                if (game.TeamBlueOdds < 0)
                {
                    game.TeamBlueOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                    if (game.TeamBlueOdds <= -100)
                    {

                    }
                    else if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = 200 - Math.Abs(game.TeamBlueOdds);
                    game.TeamRedOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                    if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = Math.Abs(game.TeamRedOdds) - 200;
                }
                if (game.TeamBlueOdds > 0)
                {
                    game.TeamBlueOdds -= (int)Math.Round(bet.CreditsBet / 8, 0);
                    if (game.TeamBlueOdds < 100)
                        game.TeamBlueOdds = Math.Abs(game.TeamBlueOdds) - 200;
                    game.TeamRedOdds += (int)Math.Round(bet.CreditsBet / 10, 0);
                    if (game.TeamRedOdds <= -100)
                    {

                    }
                    else if (game.TeamRedOdds < 100)
                        game.TeamRedOdds = 200 - Math.Abs(game.TeamRedOdds);
                }
            }
            await repository.DeleteAsync<Bet>(bet.Id);
            await repository.SaveChangesAsync();
        }



        private static decimal CalculateProfit(int odds, decimal creditsBet)
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

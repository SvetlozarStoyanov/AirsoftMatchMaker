﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class BetService : IBetService
    {
        private readonly IUnitOfWork unitOfWork;

        public BetService(IUnitOfWork repository)
        {
            this.unitOfWork = repository;
        }


        public async Task<bool> BetExistsAsync(int id)
        {
            var bet = await unitOfWork.BetRepository.GetByIdAsync(id);
            return bet != null;
        }

        public async Task<bool> UserCanAccessBetAsync(string userId, int betId)
        {
            var bet = await unitOfWork.BetRepository.GetByIdAsync(betId);
            return bet.UserId == userId;
        }
        public async Task<bool> HasUserAlreadyBetOnGameAsync(string userId, int gameId)
        {
            return await unitOfWork.BetRepository.AllReadOnly()
                .AnyAsync(b => b.UserId == userId && b.GameId == gameId);
        }

        public async Task<bool> IsUserInOneOfTheTeamsInTheGameAsync(string userId, int gameId)
        {
            var game = await unitOfWork.GameRepository.AllReadOnly()
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
            var game = await unitOfWork.GameRepository.GetByIdAsync(gameId);
            return game.GameStatus == GameStatus.Finished;
        }

        public async Task<bool> DoesGameStillAcceptBetsAsync(int gameId)
        {
            var game = await unitOfWork.GameRepository.GetByIdAsync(gameId);

            return game.Date.Date > DateTime.Now.Date;
        }

        public async Task<bool> IsUserMatchmakerAsync(string userId)
        {
            var matchmaker = await unitOfWork.MatchmakerRepository.AllReadOnly()
                .Where(mm => mm.UserId == userId)
                .FirstOrDefaultAsync();

            return matchmaker != null;
        }

        public async Task<int> GetGameIdByBetAsync(int id)
        {
            var bet = await unitOfWork.BetRepository.GetByIdAsync(id);
            return bet.GameId;
        }

        public async Task<IEnumerable<int>> GetGamesIdsWhichUserHasBetOnAsync(string userId)
        {
            var gameIds = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g => g.GameStatus == GameStatus.Upcoming && g.Bets.Any(b => b.UserId == userId))
                .Select(g => g.Id)
                .ToListAsync();
            return gameIds;
        }

        public async Task<IEnumerable<BetListModel>> GetUserBetsAsync(string userId)
        {
            var bets = await unitOfWork.BetRepository.AllReadOnly()
                .Where(b => b.UserId == userId)
                .Include(b => b.Game)
                .Select(b => new BetListModel()
                {
                    Id = b.Id,
                    GameName = b.Game.Name,
                    GameId = b.GameId,
                    CreditsBet = b.CreditsBet,
                    ChosenTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    Odds = b.Odds,
                    BetStatus = b.BetStatus,
                    GameStatus = b.Game.GameStatus
                })
                .ToListAsync();
            return bets;
        }



        public async Task<BetCreateModel> CreateBetCreateModelAsync(string userId, int gameId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            var game = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g => g.Id == gameId)
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .FirstOrDefaultAsync();

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
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            var game = await unitOfWork.GameRepository.All()
                .Where(g => g.Id == model.GameId)
                .Include(g => g.TeamBlue)
                .Include(g => g.TeamRed)
                .Include(g => g.Bets)
                .Include(g => g.GameBetCreditsContainer)
                .FirstOrDefaultAsync();


            user.Credits -= model.CreditsPlaced;

            var bet = new Bet()
            {
                UserId = userId,
                GameId = model.GameId,
                Odds = model.WinningTeamId == game.TeamRedId ? game.TeamRedOdds : game.TeamBlueOdds,
                CreditsBet = model.CreditsPlaced,
                WinningTeamId = model.WinningTeamId,
                BetStatus = BetStatus.Active
            };
            game.Bets.Add(bet);
            if (bet.WinningTeamId == game.TeamRedId)
            {
                game.GameBetCreditsContainer.TeamRedCreditsBet += bet.CreditsBet;
            }
            else if (bet.WinningTeamId == game.TeamBlueId)
            {
                game.GameBetCreditsContainer.TeamBlueCreditsBet += bet.CreditsBet;
            }
            game.OddsAreUpdated = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<BetViewModel> GetBetByIdAsync(int id)
        {
            var bet = await unitOfWork.BetRepository.AllReadOnly()
                .Where(b => b.Id == id)
                .Include(b => b.Game)
                .Select(b => new BetViewModel()
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GameId = b.GameId,
                    BetStatus = b.BetStatus,
                    GameName = b.Game.Name,
                    GameStatus = b.Game.GameStatus,
                    WinningTeamId = b.WinningTeamId,
                    WinningTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    CreditsBet = b.CreditsBet,
                    Odds = b.Odds,
                    PotentialProfit = CalculatePayout(b.Odds, b.CreditsBet),
                })
                .FirstOrDefaultAsync();
            return bet;
        }

        public async Task<BetDeleteModel> GetBetToDeleteByIdAsync(int id)
        {
            var bet = await unitOfWork.BetRepository.AllReadOnly()
                .Where(b => b.Id == id)
                .Include(b => b.User)
                .Include(b => b.Game)
                .Select(b => new BetDeleteModel()
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GameId = b.GameId,
                    GameName = b.Game.Name,
                    CreditsBet = b.CreditsBet,
                    WinningTeamId = b.WinningTeamId,
                    WinningTeamName = b.Game.TeamRedId == b.WinningTeamId ? b.Game.TeamRed.Name : b.Game.TeamBlue.Name,
                    PotentialProfit = CalculatePayout(b.Odds, b.CreditsBet),
                    Odds = b.Odds
                })
                .FirstOrDefaultAsync();

            return bet;
        }

        public async Task DeleteBetAsync(int id)
        {
            var bet = await unitOfWork.BetRepository.All()
                .Where(b => b.Id == id)
                .Include(b => b.Game)
                .ThenInclude(b => b.GameBetCreditsContainer)
                .Include(b => b.User)
                .FirstOrDefaultAsync();

            var game = bet.Game;
            if (bet.WinningTeamId == game.TeamRedId)
            {
                game.GameBetCreditsContainer.TeamRedCreditsBet -= bet.CreditsBet;
            }
            else if (bet.WinningTeamId == game.TeamBlueId)
            {
                game.GameBetCreditsContainer.TeamBlueCreditsBet -= bet.CreditsBet;
            }

            var user = bet.User;
            user.Credits += bet.CreditsBet;
            await unitOfWork.BetRepository.DeleteAsync(bet.Id);
            game.OddsAreUpdated = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task PayoutBetsByGameIdAsync(int gameId)
        {
            var game = await unitOfWork.GameRepository.All()
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
                    var profit = CalculateBetProfit(bet.Odds, bet.CreditsBet);
                    if (game.TeamRedId == winningTeamId)
                    {
                        container.TeamRedCreditsBet -= bet.CreditsBet;
                        if (container.TeamBlueCreditsBet >= profit)
                        {
                            container.TeamBlueCreditsBet -= profit;
                        }
                        else if (container.TeamBlueCreditsBet == 0)
                        {
                            profit = 0;
                        }
                        else if (container.TeamBlueCreditsBet < profit)
                        {
                            profit = container.TeamBlueCreditsBet;
                            container.TeamBlueCreditsBet = 0;
                        }
                        bet.User.Credits += bet.CreditsBet + profit;
                    }
                    else if (game.TeamBlueId == winningTeamId)
                    {
                        container.TeamBlueCreditsBet -= bet.CreditsBet;
                        if (container.TeamRedCreditsBet >= profit)
                        {
                            container.TeamRedCreditsBet -= profit;
                        }
                        else if (container.TeamRedCreditsBet == 0)
                        {
                            profit = 0;
                        }
                        else if (container.TeamRedCreditsBet < profit)
                        {
                            profit = container.TeamRedCreditsBet;
                            container.TeamRedCreditsBet = 0;
                        }
                        bet.User.Credits += bet.CreditsBet + profit;
                    }
                }
                bet.BetStatus = BetStatus.Finished;
            }
            container.BetsArePaidOut = true;
            await unitOfWork.SaveChangesAsync();
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


        private static decimal CalculatePayout(int odds, decimal creditsBet)
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

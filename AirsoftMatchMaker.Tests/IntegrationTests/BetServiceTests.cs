using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Core.Models.Bets;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class BetServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IBetService betService;
        private AirsoftMatchmakerDbContext context;

        [SetUp]
        public async Task Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<AirsoftMatchmakerDbContext>()
                .UseInMemoryDatabase("AirsoftMatchMakerTestDB")
                .Options;
            context = new AirsoftMatchmakerDbContext(contextOptions);

            context.ApplyConfiguration = false;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            unitOfWork = new UnitOfWork(context);
            betService = new BetService(unitOfWork);


            await unitOfWork.UserRepository.AddRangeAsync(new List<User>()
            {
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Paul",
                    NormalizedUserName = "PAUL",
                    Email = "PAUL@gmail.com",
                    NormalizedEmail = "PAUl@GMAIL.COM",
                },
                new User
                {
                Id = "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                UserName = "Joe",
                NormalizedUserName = "JOE",
                Email = "Joe@gmail.com",
                NormalizedEmail = "JOE@GMAIL.COM",
                Credits = 200
                },
                new User
                {
                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
                Credits = 200
                },
            });

            await unitOfWork.MatchmakerRepository.AddAsync(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });

            await unitOfWork.TeamRepository.AddRangeAsync(new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    Name = "Test1"
                },
                new Team()
                {
                    Id = 2,
                    Name = "Test2"
                },   
                new Team()
                {
                    Id = 3,
                    Name = "Test1"
                },
                new Team()
                {
                    Id = 4,
                    Name = "Test2"
                },
            });
            await unitOfWork.GameRepository.AddRangeAsync(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Now.AddDays(-1),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -122,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                },
                      new Game()
                {
                    Id = 2,
                    Name = "Test3 VS Test4",
                    Date = DateTime.Now.AddDays(1),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 3,
                    TeamBlueId = 4,
                    TeamRedOdds = -122,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 2,
                    OddsAreUpdated = true,
                }
            });
            await unitOfWork.BetRepository.AddRangeAsync(new List<Bet>()
            {
                new Bet()
                {
                    Id = 1,
                    UserId = "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 1,
                    Odds = -122
                },
                new Bet()
                {
                    Id = 2,
                    UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 2,
                    Odds = +124
                },
                 new Bet()
                {
                    Id = 3,
                    UserId = "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                    GameId = 2,
                    CreditsBet = 20,
                    WinningTeamId = 3,
                    Odds = -122
                },
            });
            await unitOfWork.GameBetCreditsContainerRepository.AddRangeAsync(new List<GameBetCreditsContainer>()
            {
                new GameBetCreditsContainer()
                {
                    Id = 1,
                    GameId = 1,
                    TeamRedCreditsBet = 20,
                    TeamBlueCreditsBet = 20,
                },
                new GameBetCreditsContainer()
                {
                    Id = 2,
                    GameId = 2,
                    TeamRedCreditsBet = 20,
                    TeamBlueCreditsBet = 0,
                },
            });

            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_CreateBetAsync_WorksCorrectly()
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync("c5d9e543-7c2f-4345-a014-ebd860eef718");
            var betCreditsContainer = await unitOfWork.GameBetCreditsContainerRepository.GetByIdAsync(2);
            var model = new BetCreateModel()
            {
                GameId = 2,
                CreditsPlaced = 20,
                WinningTeamId = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
            };
            await betService.CreateBetAsync("c5d9e543-7c2f-4345-a014-ebd860eef718", model);
            var game = await unitOfWork.GameRepository.GetByIdAsync(2);
            Assert.That(user.Credits, Is.EqualTo(180));
            Assert.That(betCreditsContainer.TeamRedCreditsBet, Is.EqualTo(20));
            Assert.That(game.OddsAreUpdated, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_DeleteBetAsync_WorksCorrectly()
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a");
            var betCreditsContainer = await unitOfWork.GameBetCreditsContainerRepository.GetByIdAsync(2);

            await betService.DeleteBetAsync(3);
            var game = await unitOfWork.GameRepository.GetByIdAsync(2);
            Assert.That(user.Credits, Is.EqualTo(220));
            Assert.That(betCreditsContainer.TeamRedCreditsBet, Is.EqualTo(0));
            Assert.That(game.OddsAreUpdated, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_PayoutBetsAsync_WorksCorrectly()
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a");
            var betCreditsContainer = await unitOfWork.GameBetCreditsContainerRepository.GetByIdAsync(1);
            
            var game = await unitOfWork.GameRepository.GetByIdAsync(1);
            game.Result = "3:2";
            game.TeamRedPoints = 3;
            game.TeamBluePoints = 2;
            await betService.PayoutBetsByGameIdAsync(1);
            Assert.That(user.Credits, Is.EqualTo(200 + CalculatePayout(-122,20)));
            Assert.That(betCreditsContainer.TeamRedCreditsBet, Is.EqualTo(0));
            Assert.That(betCreditsContainer.BetsArePaidOut, Is.EqualTo(true));
        }




        [TearDown]
        public void Teardown()
        {
            context.Dispose();
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

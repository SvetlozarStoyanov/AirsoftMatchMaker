using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
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

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class BetServiceTests
    {
        private IRepository repository;
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

            repository = new Repository(context);
            betService = new BetService(repository);

            await repository.AddRangeAsync<Bet>(new List<Bet>()
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
            });

            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_BetExistsAsync_ReturnsTrueWhenBetExists()
        {
            var result = await betService.BetExistsAsync(1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_BetExistsAsync_ReturnsFalseWhenDoesNotExist()
        {
            var result = await betService.BetExistsAsync(30);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_IsGameFinished_ReturnsTrueWhenGameIsFinished()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });

            await repository.SaveChangesAsync();
            var result = await betService.IsGameFinishedAsync(1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_IsGameFinished_ReturnsFalseWhenGameIsNotFinished()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });

            await repository.SaveChangesAsync();
            var result = await betService.IsGameFinishedAsync(1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_DoesGameStillAcceptBetsAsync_ReturnTrueWhenGameAcceptsBets()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Now.AddDays(1),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });
            var result = await betService.DoesGameStillAcceptBetsAsync(1);
            Assert.That(result, Is.EqualTo(true));

        }

        [Test]
        public async Task Test_DoesGameStillAcceptBetsAsync_ReturnFalseWhenGameDoesNotAcceptBets()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Now,
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });
            var result = await betService.DoesGameStillAcceptBetsAsync(1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_IsUserMatchmakerAsync_ReturnTrueWhenUserIsMatchmaker()
        {
            await repository.AddRangeAsync<User>(new List<User>()
            {
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Matchmaker",
                    NormalizedUserName = "MATCHMAKER",
                    Email = $"Matchmaker@gmail.com",
                    NormalizedEmail = $"MATCHMAKER@GMAIL.COM",
                },
            });
            await repository.AddAsync<Matchmaker>(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await repository.SaveChangesAsync();
            var result = await betService.IsUserMatchmakerAsync("c5d9e543-7c2f-4345-a014-ebd860eef718");
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_IsUserMatchmakerAsync_ReturnFalseWhenUserIsNotMatchmaker()
        {
            await repository.AddRangeAsync<User>(new List<User>()
            {
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Matchmaker",
                    NormalizedUserName = "MATCHMAKER",
                    Email = $"Matchmaker@gmail.com",
                    NormalizedEmail = $"MATCHMAKER@GMAIL.COM",
                },
                new User()
                {
                    Id = "0274ded9-003a-48eb-b608-7477597ec876",
                    UserName = "Player1",
                    NormalizedUserName = "PLAYER1",
                    Email = $"Player1@gmail.com",
                    NormalizedEmail = $"PLAYER1@GMAIL.COM",
                },
            });
            await repository.AddAsync<Matchmaker>(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await repository.SaveChangesAsync();
            var result = await betService.IsUserMatchmakerAsync("0274ded9-003a-48eb-b608-7477597ec876");
            Assert.That(result, Is.EqualTo(false));
        }

        [TearDown]
        public void Teardown()
        {
            context.Dispose();
        }
    }
}

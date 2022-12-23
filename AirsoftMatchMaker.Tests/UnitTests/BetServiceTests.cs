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

            await repository.AddRangeAsync<User>(new List<User>()
            {
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

            await repository.AddRangeAsync<Game>(new List<Game>()
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
                }
            });
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
        public async Task Test_UserCanAccessBetAsync_BetCreatorCanAccess()
        {
            var result = await betService.UserCanAccessBetAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a", 1);
            Assert.That(result, Is.EqualTo(true));
        }
        [Test]
        public async Task Test_UserCanAccessBetAsync_DifferentUserCannotAccess()
        {
            var result = await betService.UserCanAccessBetAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_HasUserAlreadyBetOnGame_ReturnsFalseWhenUserHasntBet()
        {
            var result = await betService.HasUserAlreadyBetOnGameAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_HasUserAlreadyBetOnGame_ReturnsTrueWhenUserHasBet()
        {
            var result = await betService.HasUserAlreadyBetOnGameAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a", 1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_IsUserInOneOfTheTeamsInTheGameAsync_ReturnsFalseWhenUserIsnt()
        {
            await repository.AddRangeAsync<User>(new List<User>()
            {
                new User()
                {
                    Id = "0274ded9-003a-48eb-b608-7477597ec876",
                    UserName = "Player1",
                    NormalizedUserName = "PLAYER1",
                    Email = $"Player1@gmail.com",
                    NormalizedEmail = $"PLAYER1@GMAIL.COM",
                },
                new User()
                {
                    Id = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    UserName = "Player2",
                    NormalizedUserName = "PLAYER2",
                    Email = $"Player2@gmail.com",
                    NormalizedEmail = $"PLAYER2@GMAIL.COM",
                },
            });
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.AddRangeAsync<Player>(new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    UserId = "0274ded9-003a-48eb-b608-7477597ec876",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 1,
                    IsActive = true,
                    SkillPoints = 200,
                },
                new Player()
                {
                    Id = 2,
                    UserId = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 2,
                    IsActive = true,
                    SkillPoints = 200,
                }
            });
            await repository.SaveChangesAsync();
            var result = await betService.IsUserInOneOfTheTeamsInTheGameAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a", 1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_IsUserInOneOfTheTeamsInTheGameAsync_ReturnsTrueWhenUserIs()
        {
            await repository.AddRangeAsync<User>(new List<User>()
            {
                new User()
                {
                    Id = "0274ded9-003a-48eb-b608-7477597ec876",
                    UserName = "Player1",
                    NormalizedUserName = "PLAYER1",
                    Email = $"Player1@gmail.com",
                    NormalizedEmail = $"PLAYER1@GMAIL.COM",
                },
                new User()
                {
                    Id = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    UserName = "Player2",
                    NormalizedUserName = "PLAYER2",
                    Email = $"Player2@gmail.com",
                    NormalizedEmail = $"PLAYER2@GMAIL.COM",
                },
            });
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.AddRangeAsync<Player>(new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    UserId = "0274ded9-003a-48eb-b608-7477597ec876",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 1,
                    IsActive = true,
                    SkillPoints = 200,
                },
                new Player()
                {
                    Id = 2,
                    UserId = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 2,
                    IsActive = true,
                    SkillPoints = 200,
                }
            });
            await repository.SaveChangesAsync();
            var result = await betService.IsUserInOneOfTheTeamsInTheGameAsync("0b532089-f327-4dfa-a718-bc8bf8bad9a5", 1);
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public async Task Test_IsGameFinished_ReturnsTrueWhenGameIsFinished()
        {
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
                    Id = 2,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Now.AddDays(2),
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
            var result = await betService.IsGameFinishedAsync(2);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_DoesGameStillAcceptBetsAsync_ReturnTrueWhenGameAcceptsBets()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 2,
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
            var result = await betService.DoesGameStillAcceptBetsAsync(2);
            Assert.That(result, Is.EqualTo(true));

        }

        [Test]
        public async Task Test_DoesGameStillAcceptBetsAsync_ReturnFalseWhenGameDoesNotAcceptBets()
        {
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 2,
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

        [Test]
        public async Task Test_GetGameIdByBetAsync_ReturnsCorrectId()
        {
            var result = await betService.GetGameIdByBetAsync(1);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetUserBetsAsync_ReturnsCorrectBets()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.SaveChangesAsync();
            var result = await betService.GetUserBetsAsync("cc1cb39b-c0cf-41ed-856c-d3943aec605a");
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.ElementAt(0).Id, Is.EqualTo(1));

            result = await betService.GetUserBetsAsync("77388c0c-698c-4df9-9ad9-cef29116b666");
            Assert.That(result.Count(), Is.EqualTo(0));
        }
        [Test]
        public async Task Test_CreateBetCreateModelAsync_ReturnsCorrectModel()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.SaveChangesAsync();
            var model = await betService.CreateBetCreateModelAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 1);

            Assert.That(model.UserCredits, Is.EqualTo(200));
            Assert.That(model.TeamRedName, Is.EqualTo("Test1"));
            Assert.That(model.TeamBlueName, Is.EqualTo("Test2"));
            Assert.That(model.TeamRedOdds, Is.EqualTo(-122));
            Assert.That(model.TeamBlueOdds, Is.EqualTo(+124));
        }

        [Test]
        public async Task Test_GetBetByIdAsync_ReturnsCorrectModel()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.SaveChangesAsync();
            var model = await betService.GetBetByIdAsync(1);

            Assert.That(model.CreditsBet, Is.EqualTo(20));
            Assert.That(model.WinningTeamId, Is.EqualTo(1));
            Assert.That(model.Odds, Is.EqualTo(-122));
            Assert.That(model.UserId, Is.EqualTo("cc1cb39b-c0cf-41ed-856c-d3943aec605a"));
        }        
        [Test]
        public async Task Test_GetBetToDeleteByIdAsync_ReturnsCorrectModel()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            });
            await repository.SaveChangesAsync();
            var model = await betService.GetBetToDeleteByIdAsync(1);

            Assert.That(model.CreditsBet, Is.EqualTo(20));
            Assert.That(model.GameName, Is.EqualTo("Test1 VS Test2"));
            Assert.That(model.WinningTeamId, Is.EqualTo(1));
            Assert.That(model.Odds, Is.EqualTo(-122));
            Assert.That(model.UserId, Is.EqualTo("cc1cb39b-c0cf-41ed-856c-d3943aec605a"));
        }

        [TearDown]
        public void Teardown()
        {
            context.Dispose();
        }
    }
}

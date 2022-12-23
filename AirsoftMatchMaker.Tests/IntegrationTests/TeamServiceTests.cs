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
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Teams;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class TeamServiceTests
    {
        private IRepository repository;
        private ITeamService teamService;
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
            teamService = new TeamService(repository);

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
            await repository.AddRangeAsync<User>(new List<User>()
            {
                new User
                {
                    Id = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    UserName = "Ivan",
                    NormalizedUserName = "IVAN",
                    Email = "Ivan@gmail.com",
                    NormalizedEmail = "IVAN@GMAIL.COM",
                    Credits = 200
                },
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
            await repository.AddRangeAsync<Player>(new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    Ammo = 200,
                    UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    TeamId = 1,
                    PlayerClassId = 1,
                    IsActive = true
                },
                new Player()
                {
                    Id = 2,
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
                    Id = 3,
                    UserId = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = null,
                    IsActive = true,
                    SkillPoints = 200,
                },
            });
            await repository.AddAsync<GameMode>(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                PointsToWin = 3,
                Description = "",

            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

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
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    TeamRedPoints = 2,
                    TeamBluePoints = 3,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                },
                new Game()
                {
                    Id = 2,
                    Name = "Test2 VS Test1",
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
                    OddsAreUpdated = false,
                }
            });
            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetTeamByIdAsync_ReturnsCorrectModel()
        {
            var model = await teamService.GetTeamByIdAsync(1);
            Assert.That(model.Id, Is.EqualTo(1));
            Assert.That(model.Games.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetPlayersTeamAsync_ReturnsCorrectModel()
        {
            var model = await teamService.GetPlayersTeamAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            Assert.That(model.Id, Is.EqualTo(1));
            Assert.That(model.Games.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_CreateTeamAsync_CreatesTeamSuccessfully()
        {
            var model = new TeamCreateModel()
            {
                Name = "Test3",
            };
            await teamService.CreateTeamAsync("0b532089-f327-4dfa-a718-bc8bf8bad9a5",model);
            var newTeam = await repository.GetByIdAsync<Team>(3);
            Assert.That(newTeam.Id, Is.EqualTo(3));
            Assert.That(newTeam.Players.Count, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }

}

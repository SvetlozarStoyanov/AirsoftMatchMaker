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

namespace AirsoftMatchMaker.Tests.UnitTests
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

            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_TeamExistsAsync_ReturnsTrueWhenTeamExists()
        {
            var result = await teamService.TeamExistsAsync(1);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_TeamExistsAsync_ReturnsFalseWhenTeamDoesNotExist()
        {
            var result = await teamService.TeamExistsAsync(4);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_DoesUserHaveTeamAsync_ReturnsTrueWhenUserHasTeam()
        {
            var result = await teamService.DoesUserHaveTeamAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            Assert.IsTrue(result);
        }
        [Test]
        public async Task Test_DoesUserHaveTeamAsync_ReturnsFalseWhenUserHasNoTeam()
        {
            var result = await teamService.DoesUserHaveTeamAsync("0b532089-f327-4dfa-a718-bc8bf8bad9a5");
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_DoesTeamWithSameNameExistAsync_ReturnsTrueWhenOtherTeamWithSameNameExists()
        {
            var result = await teamService.DoesTeamWithSameNameExistAsync("Test1");
            Assert.IsTrue(result);
        }
        [Test]
        public async Task Test_DoesTeamWithSameNameExistAsync_ReturnsFalseWhenNoOtherTeamWithSameNameExists()
        {
            var result = await teamService.DoesTeamWithSameNameExistAsync("Test76");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_GetAllTeamsAsync_ReturnsCorrectTeams()
        {
            var result = await teamService.GetAllTeamsAsync(
                null,
                TeamSorting.Newest,
                6,
                1
                );
            Assert.That(result.TeamsCount, Is.EqualTo(2));
            result = await teamService.GetAllTeamsAsync(
                "2",
                TeamSorting.Newest,
                6,
                1
                );
            Assert.That(result.TeamsCount, Is.EqualTo(1));

        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }

}

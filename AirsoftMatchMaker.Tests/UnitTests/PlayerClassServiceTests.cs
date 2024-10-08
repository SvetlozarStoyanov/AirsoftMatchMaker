using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftMatchMaker.Core.Services;
using Microsoft.EntityFrameworkCore;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class PlayerClassServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IPlayerClassService playerClassService;
        private AirsoftMatchmakerDbContext context;
        [SetUp]
        public async Task SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<AirsoftMatchmakerDbContext>()
            .UseInMemoryDatabase("AirsoftMatchMakerTestDB")
            .Options;
            context = new AirsoftMatchmakerDbContext(contextOptions);

            context.ApplyConfiguration = false;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            unitOfWork = new UnitOfWork(context);
            playerClassService = new PlayerClassService(unitOfWork);

            await unitOfWork.PlayerClassRepository.AddRangeAsync(new List<PlayerClass>()
            {
                new PlayerClass()
                {
                    Id = 1,
                    Name = "New Player",
                    Description = "New to the game. Prone to make mistakes."
                },
                new PlayerClass()
                {
                    Id = 2,
                    Name = "Leader",
                    Description = "Provides good advice and coordinates teams well."
                },
                new PlayerClass()
                {
                    Id = 3,
                    Name = "Frontline",
                    Description = "Always goes first. Good in both defence and offence."
                },
                new PlayerClass()
                {
                    Id = 4,
                    Name = "Marksman",
                    Description = "High accuracy over long range. Struggles in close range."
                },
                new PlayerClass()
                {
                    Id = 5,
                    Name = "Sneaky",
                    Description = "Loves to sneak behind and surprise enemy teams from behind."
                },
                new PlayerClass()
                {
                    Id = 6,
                    Name = "Camper",
                    Description = "Excels in defending, lacks in attacking."
                },
                new PlayerClass()
                {
                    Id = 7,
                    Name = "Rusher",
                    Description = "Excels in attacking, lacks in defending."
                }
            });
            await unitOfWork.UserRepository.AddRangeAsync(new List<User>()
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
            });
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_PlayerClassExists_ReturnsTrueWhenPlayerClassExists()
        {
            var result = await playerClassService.PlayerClassExists(1);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_PlayerClassExists_ReturnsFalseWhenPlayerClassDoesNotExist()
        {
            var result = await playerClassService.PlayerClassExists(500);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsPlayerAlreadyInPlayerClass_ReturnsTrueWhenPlayerIsAlreadyInClass()
        {
            var result = await playerClassService.IsPlayerAlreadyInPlayerClass("202efe8b-7748-49ca-834c-fd1c37978ab2", 1);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsPlayerAlreadyInPlayerClass_ReturnsFalseWhenPlayerNotInClass()
        {
            var result = await playerClassService.IsPlayerAlreadyInPlayerClass("202efe8b-7748-49ca-834c-fd1c37978ab2", 5);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_GetAllPlayerClassesAsync_ReturnsAllPlayerClasses()
        {
            var result = await playerClassService.GetAllPlayerClassesAsync();
            var playerClasses = await unitOfWork.PlayerClassRepository.AllReadOnly().ToListAsync();
            Assert.That(result.Count(), Is.EqualTo(playerClasses.Count - 1));
        }
        [Test]
        public async Task Test_GetPlayersPlayerClassIdByUserIdAsync_ReturnsCorrectId()
        {
            var result = await playerClassService.GetPlayersPlayerClassIdByUserIdAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public async Task Test_ChangePlayerClassAsync_ChangesPlayerClassCorrectly()
        {
            var player = await unitOfWork.PlayerRepository.GetByIdAsync(1);
            await playerClassService.ChangePlayerClassAsync(player.UserId, 3);
            Assert.That(player.PlayerClassId, Is.EqualTo(3));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}

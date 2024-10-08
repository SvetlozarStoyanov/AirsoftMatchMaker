using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class GameModeServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IGameModeService gameModeService;
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
            gameModeService = new GameModeService(unitOfWork);

            await unitOfWork.GameModeRepository.AddRangeAsync(new List<GameMode>()
            {
                new GameMode()
                {
                    Id = 1,
                    Name = "Capture the flag",
                    Description = "Whoever captures the flag first scores a point.",
                    PointsToWin = 3
                },
                new GameMode()
                {
                    Id = 2,
                    Name = "Secure area",
                    Description = "The team which controls the point in the middle for 5 minutes wins.",
                    PointsToWin = 2
                }
            });
            await unitOfWork.SaveChangesAsync();
        }



        [Test]
        public async Task Test_GameModeExistsAsync_ReturnsTrueWhenGameModeExists()
        {
            var result = await gameModeService.GameModeExistsAsync(1);
            Assert.True(result);
        }

        [Test]
        public async Task Test_GameModeExistsAsync_ReturnsFalseWhenGameModeDoesNotExist()
        {
            var result = await gameModeService.GameModeExistsAsync(3);
            Assert.False(result);
        }

        [Test]
        public async Task Test_IsGameModeNameAlreadyTaken_ReturnsTrueWhenGameModeExists()
        {
            var result = await gameModeService.IsGameModeNameAlreadyTaken("Secure area");
            Assert.True(result);
        }

        [Test]
        public async Task Test_IsGameModeNameAlreadyTaken_ReturnsFalseWhenGameModeDoesNotExist()
        {
            var result = await gameModeService.IsGameModeNameAlreadyTaken("King of the hill");
            Assert.False(result);
        }


        [Test]
        public async Task Test_CreateGameModeAsync_CreatesGameModeSuccessfully()
        {
            var model = new GameModeCreateModel()
            {
                Name = "Test",
                Description = "Test desc",
                PointsToWin = 3,
            };
            await gameModeService.CreateGameModeAsync(model);
            var gameModes = await unitOfWork.GameModeRepository.AllReadOnly().ToListAsync();
            Assert.That(gameModes.Count, Is.EqualTo(3));
        }
        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }

    }
}

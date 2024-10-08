using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class GameServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IGameService gameService;
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
            gameService = new GameService(unitOfWork);

            await unitOfWork.GameRepository.AddRangeAsync(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
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
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetGameByIdAsync_ReturnsNullWhenIdDoesNotExist()
        {
            await unitOfWork.SaveChangesAsync();
            var gameModel = await gameService.GetGameByIdAsync(1);
            Assert.That(gameModel, Is.EqualTo(null));
        }

        [Test]
        public async Task Test_GameExistsAsync_ReturnsTrueWhenGameExists()
        {
            var gameExists = await gameService.GameExistsAsync(1);
            Assert.That(gameExists, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_GameExistsAsync_ReturnsFalseWhenGameDoesNotExist()
        {
            var gameExists = await gameService.GameExistsAsync(30);
            Assert.That(gameExists, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_GameCanBeFinalisedAsync_ReturnsTrueWhenUserIdIsCorrect()
        {
            await unitOfWork.UserRepository.AddRangeAsync(new List<User>()
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
            await unitOfWork.MatchmakerRepository.AddAsync(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await unitOfWork.SaveChangesAsync();
            var result = await gameService.GameCanBeFinalisedAsync("c5d9e543-7c2f-4345-a014-ebd860eef718", 1);
            
            Assert.That(result, Is.EqualTo(true));
        }


        [Test]
        public async Task Test_GameCanBeFinalisedAsync_ReturnsTrueWhenUserIdIsIncorrect()
        {
            await unitOfWork.UserRepository.AddRangeAsync(new List<User>()
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
            await unitOfWork.MatchmakerRepository.AddAsync(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await unitOfWork.SaveChangesAsync();
            var result = await gameService.GameCanBeFinalisedAsync("0274ded9-003a-48eb-b608-7477597ec876", 1);

            Assert.That(result, Is.EqualTo(false));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
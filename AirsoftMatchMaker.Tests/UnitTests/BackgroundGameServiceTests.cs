using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    internal class BackgroundGameServiceTests
    {
        private AirsoftMatchmakerDbContext context;
        private IUnitOfWork unitOfWork;
        private BackgroundGameService backgroundGameservice;
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
            backgroundGameservice = new BackgroundGameService(unitOfWork);

            await unitOfWork.GameRepository.AddRangeAsync(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Today.AddHours(-7),
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
                },
                new Game()
                {
                    Id = 2,
                    Name = "Test3 VS Test4",
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
                    OddsAreUpdated = false,
                }
            });
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_MarkGamesAsFinishedAsync_MarksCorrectGames()
        {
            await backgroundGameservice.MarkGamesAsFinishedAsync(DateTime.Now);
            var games = await unitOfWork.GameRepository.AllReadOnly()
                .ToListAsync();

            Assert.That(games.Count(g => g.GameStatus == GameStatus.Upcoming), Is.EqualTo(1));
            Assert.That(games.Count(g => g.GameStatus == GameStatus.Finished), Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetGameIdsOfGamesWithNotUpDoDateOddsAsync_ReturnsCorrectIds()
        {
           var gameIds =  await backgroundGameservice.GetGameIdsOfGamesWithNotUpDoDateOddsAsync();

            Assert.That(gameIds.Count(), Is.EqualTo(1));
            Assert.That(gameIds.First(), Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }

}

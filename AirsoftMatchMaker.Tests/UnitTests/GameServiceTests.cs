using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AngleSharp.Text;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class GameServiceTests
    {
        private IRepository repository;
        private IGameService gameService;
        private AirsoftMatchmakerDbContext context;

        [SetUp]
        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<AirsoftMatchmakerDbContext>()
                .UseInMemoryDatabase("AirsoftMatchMakerTestDB")
                .Options;
            context = new AirsoftMatchmakerDbContext(contextOptions);

            context.ApplyConfiguration = false;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repository = new Repository(context);
            gameService = new GameService(repository);
        }



        [Test]
        public async Task TestGetGameByIdReturnsNullWhenIdDoesNotExist()
        {
            await repository.SaveChangesAsync();
            var gameModel = await gameService.GetGameByIdAsync(1);
            Assert.That(gameModel, Is.EqualTo(null));
        }
    }
}
using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AirsoftMatchMaker.Core.Models.Enums;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class MapServiceTests
    {
        private IRepository repository;
        private IMapService mapService;
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
            mapService = new MapService(repository);


            await repository.AddRangeAsync<GameMode>(new List<GameMode>()
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
                new Team()
                {
                    Id = 3,
                    Name = "Test3"
                },
                new Team()
                {
                    Id = 4,
                    Name = "Test4"
                },
            });
            await repository.AddRangeAsync<Map>(new List<Map>()
            {
                new Map
                {
                    Id = 1,
                    Name = "Forest",
                    Description = "Large forest map in Norway",
                    ImageUrl = "https://cdn.britannica.com/87/138787-050-33727493/Belovezhskaya-Forest-Poland.jpg",
                    Terrain = TerrainType.Forest,
                    AverageEngagementDistance = AverageEngagementDistance.Medium,
                    Mapsize = Mapsize.Large,
                    GameModeId = 1
                },
                new Map
                {
                    Id = 2,
                    Name = "Clear Field",
                    Description = "Small Field in California",
                    ImageUrl = "https://www.arboursabroad.com/westflanders_be_110318-56/",
                    Terrain = TerrainType.Field,
                    AverageEngagementDistance = AverageEngagementDistance.Short,
                    Mapsize = Mapsize.Small,
                    GameModeId = 2
                },
            });
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 vs Test2",
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
                    OddsAreUpdated = true,
                },
                new Game()
                {
                    Id = 2,
                    Name = "Test3 vs Test4",
                    Date = DateTime.Now.AddDays(-1),
                    EntryFee = 40,
                    MatchmakerId = 1,
                    GameModeId = 2,
                    GameStatus = GameStatus.Finished,
                    MapId = 2,
                    TeamRedId = 3,
                    TeamBlueId = 4,
                    TeamRedOdds = +140,
                    TeamBlueOdds = -140,
                    OddsAreUpdated = true,
                },
            });
            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetAllMapsAsync_ReturnsCorrectMaps()
        {
            var result = await mapService.GetAllMapsAsync(
                null,
                null,
                MapSorting.Newest,
                6,
                1
                );
            Assert.That(result.MapsCount, Is.EqualTo(2));
            result = await mapService.GetAllMapsAsync(
                null,
                "Secure Area",
                MapSorting.Newest,
                6,
                1
                );
            Assert.That(result.MapsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetMapByIdAsync_ReturnsCorrectModel()
        {
            var model = await mapService.GetMapByIdAsync(1);
            Assert.That(model.Id, Is.EqualTo(1));
            Assert.That(model.Name, Is.EqualTo("Forest"));
            Assert.That(model.Games.Count(), Is.EqualTo(1));
            Assert.That(model.GameModeId, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}

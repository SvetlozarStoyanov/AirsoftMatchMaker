using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.GameModes;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class MapServiceTests
    {
        private IUnitOfWork unitOfWork;
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

            unitOfWork = new UnitOfWork(context);
            mapService = new MapService(unitOfWork);

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
            await unitOfWork.MapRepository.AddRangeAsync(new List<Map>()
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
            await unitOfWork.SaveChangesAsync();
        }



        [Test]
        public async Task Test_MapExistsAsync_ReturnsTrueWhenGameModeExists()
        {
            var result = await mapService.MapExistsAsync(1);
            Assert.True(result);
        }

        [Test]
        public async Task Test_MapExistsAsync_ReturnsFalseWhenGameModeDoesNotExist()
        {
            var result = await mapService.MapExistsAsync(3);
            Assert.False(result);
        }

        [Test]
        public async Task Test_IsMapNameAlreadyTaken_ReturnsTrueWhenGameModeExists()
        {
            var result = await mapService.IsMapNameAlreadyTaken("Forest");
            Assert.True(result);
        }

        [Test]
        public async Task Test_IsMapNameAlreadyTaken_ReturnsFalseWhenGameModeDoesNotExist()
        {
            var result = await mapService.IsMapNameAlreadyTaken("Mountain");
            Assert.False(result);
        }

        [Test]
        public async Task Test_CreateMapCreateModelAsync_ReturnsCorrectModel()
        {
            var model = await mapService.CreateMapCreateModelAsync();
            Assert.That(model.GameModeIds.Count, Is.EqualTo(2));
            Assert.That(model.Mapsizes.Count, Is.EqualTo(4));
            Assert.That(model.AverageEngagementDistances.Count, Is.EqualTo(3));
        }
        [Test]
        public async Task Test_CreateMapAsync_CreatesMapSuccessfully()
        {
            var model = new MapCreateModel()
            {
                Name = "Test",
                Description = "Test desc",
                AverageEngagementDistance = AverageEngagementDistance.Short,
                Mapsize = Mapsize.Small,
                GameModeId = 1,
            };
            await mapService.CreateMapAsync(model);
            var maps = await unitOfWork.MapRepository.AllReadOnly().ToListAsync();
            Assert.That(maps.Count, Is.EqualTo(3));
        }
        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }

    }
}

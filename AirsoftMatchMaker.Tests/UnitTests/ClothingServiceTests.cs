using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class ClothingServiceTests
    {
        private IRepository repository;
        private IClothingService clothingService;
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
            clothingService = new ClothingService(repository);

            await repository.AddRangeAsync<Clothing>(new List<Clothing>()
            {
                new Clothing()
                {
                    Id = 1,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = 2
                },
            });
            await repository.SaveChangesAsync();
        }



        [Test]
        public async Task Test_GetAmmoBoxByIdAsync_ReturnsNullWhenIdDoesNotExist()
        {
            await repository.SaveChangesAsync();
            var gameModel = await clothingService.GetClothingByIdAsync(30);
            Assert.That(gameModel, Is.EqualTo(null));
        }

        [Test]
        public async Task Test_AmmoBoxExistsAsync_ReturnsTrueWhenAmmoBoxExists()
        {
            var result = await clothingService.ClothingExistsAsync(1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_ClothingExistsAsync_ReturnsFalseWhenClothingDoesNotExist()
        {
            var result = await clothingService.ClothingExistsAsync(30);
            Assert.That(result, Is.EqualTo(false));
        }


        [Test]
        public async Task Test_IsClothingForSaleAsync_ReturnsTrueWhenClothingIsForSale()
        {
            await repository.AddRangeAsync<Clothing>(new List<Clothing>()
            {
                new Clothing()
                {
                    Id = 2,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    VendorId = 1
                },
            });
            await repository.SaveChangesAsync();
            var result = await clothingService.ClothingIsForSaleAsync(2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_IsClothingForSaleAsync_ReturnsFalseWhenClothingIsNotForSale()
        {
        
            var result = await clothingService.ClothingIsForSaleAsync(1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_GetClothingListModelForBuyAsync_ReturnsModelCorrectly()
        {
            var resultModel = await clothingService.GetClothingListModelForBuyAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Green outfit"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.ClothingColor, Is.EqualTo(ClothingColor.Green));
            Assert.That(resultModel.Price, Is.EqualTo(50));
        }

        [Test]
        public async Task Test_CreateClothingSellModelAsync_ReturnsModelCorrectly()
        {
            var resultModel = await clothingService.CreateClothingSellModelAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Green outfit"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.ClothingColor, Is.EqualTo(ClothingColor.Green));
            Assert.That(resultModel.Price, Is.EqualTo(50));
            Assert.That(resultModel.OldPrice, Is.EqualTo(50));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
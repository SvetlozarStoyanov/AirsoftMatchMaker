using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class ClothingServiceTests
    {
        private IUnitOfWork unitOfWork;
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

            unitOfWork = new UnitOfWork(context);
            clothingService = new ClothingService(unitOfWork);

            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
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
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_ClothingExistsAsync_ReturnsTrueWhenAmmoBoxExists()
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
        public async Task Test_UserCanBuyClothingAsync_ReturnsTrueWhenUserCanBuyClothing()
        {
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
                new User
                {

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
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
            await unitOfWork.VendorRepository.AddAsync(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666"
            });
            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
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
            await unitOfWork.SaveChangesAsync();

            var result = await clothingService.UserCanBuyClothingAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserCanBuyClothingAsync_ReturnsFalseWhenUserCannotBuyClothing()
        {
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
                new User
                {

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
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
            await unitOfWork.VendorRepository.AddAsync(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = false
            });
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
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
            await unitOfWork.SaveChangesAsync();

            var result = await clothingService.UserCanBuyClothingAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_UserCanSellClothingAsync_ReturnsTrueWhenUserCanSellClothing()
        {
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
                new User
                {

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
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
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await unitOfWork.VendorRepository.AddAsync(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = true
            });
            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
            {
                new Clothing()
                {
                    Id = 2,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = 2
                },
            });
            await unitOfWork.SaveChangesAsync();

            var result = await clothingService.UserCanSellClothingAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserCanSellClothingAsync_ReturnsFalseWhenUserCannotSellClothing()
        {
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
                new User
                {

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
                Credits = 200
                }
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
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await unitOfWork.VendorRepository.AddAsync(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = true
            });
            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
            {
                new Clothing()
                {
                    Id = 2,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = 1
                },
            });
            await unitOfWork.SaveChangesAsync();

            var result = await clothingService.UserCanSellClothingAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_IsClothingForSaleAsync_ReturnsTrueWhenClothingIsForSale()
        {
            await unitOfWork.ClothingRepository.AddRangeAsync(new List<Clothing>()
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
            await unitOfWork.SaveChangesAsync();
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

        [Test]
        public async Task Test_GetClothingByIdAsync_ReturnsCorrectModel()
        {
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
                new User
                {

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
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
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await unitOfWork.SaveChangesAsync();
            var gameModel = await clothingService.GetClothingByIdAsync(1);
            Assert.That(gameModel.Id, Is.EqualTo(1));
            Assert.That(gameModel.PlayerId, Is.EqualTo(2));
            Assert.That(gameModel.PlayerName, Is.EqualTo("Vendor"));
        }


        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
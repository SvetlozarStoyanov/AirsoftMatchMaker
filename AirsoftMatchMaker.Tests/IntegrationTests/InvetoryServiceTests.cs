using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class InvetoryServiceTests
    {
        private IRepository repository;
        private IInventoryService invetoryService;
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
            invetoryService = new InventoryService(repository);

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
                    Ammo = 200,
                    UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                    TeamId = 1,
                    PlayerClassId = 1,
                    IsActive = false
                }
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666"
            });
            await repository.AddRangeAsync<AmmoBox>(new List<AmmoBox>()
            {
                new AmmoBox()
                {
                    Id = 1,
                    Name = "Small box",
                    Amount = 50,
                    Price = 10,
                    Quantity = 900,
                    VendorId = 1
                },
                 new AmmoBox()
                {
                    Id = 2,
                    Name = "Big box",
                    Amount = 500,
                    Price = 100,
                    Quantity = 800,
                    VendorId = 1
                },

            });
            await repository.AddRangeAsync<Clothing>(new List<Clothing>()
            {
                new Clothing()
                {
                    Id = 1,
                    Name = "Green clothing",
                    Description = "Hard to spot in forests",
                    Price = 10,
                    ClothingColor = ClothingColor.Green,
                    VendorId = 1
                },
                 new Clothing()
                {
                    Id = 2,
                    Name = "Gray clothing",
                    Description = "Hard to spot in urban enviroment",
                    ClothingColor = ClothingColor.Gray,
                    Price = 100,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 3,
                    Name = "Gray clothing",
                    Description = "Hard to spot in urban enviroment",
                    ClothingColor = ClothingColor.Gray,
                    Price = 100,
                    PlayerId = 2,
                },
            });
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
                new Weapon()
                {
                    Id = 1,
                    Name = "Glock 18",
                    Description = "Automatic pistol",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    VendorId = 1
                },
                new Weapon()
                {
                    Id = 2,
                    Name = "AK47",
                    Description = "Soviet rifle",
                    ImageUrl = null,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 100,
                    VendorId = 1
                },

                 new Weapon()
                {
                    Id = 3,
                    Name = "M4A1",
                    Description = "Soviet rifle",
                    ImageUrl = null,
                    WeaponType = WeaponType.AssaultRifle,
                    Price = 100,
                    PlayerId = 2
                },
            });
            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetVendorItemsAsync_ReturnsCorrectItems()
        {
            var result = await invetoryService.GetVendorItemsAsync("77388c0c-698c-4df9-9ad9-cef29116b666");
            Assert.That(result.AmmoBoxes.Count, Is.EqualTo(2));
            Assert.That(result.Clothes.Count, Is.EqualTo(2));
            Assert.That(result.Weapons.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Test_GetPlayerItemsAsync_ReturnsCorrectItems()
        {
            var result = await invetoryService.GetPlayerItemsAsync("77388c0c-698c-4df9-9ad9-cef29116b666");
            Assert.That(result.Clothes.Count, Is.EqualTo(1));
            Assert.That(result.Weapons.Count, Is.EqualTo(1));
        }


        [TearDown]
        public void TearDown()
        {
            this.context.Dispose();
        }
    }
}

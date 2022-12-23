using AirsoftMatchMaker.Core.Common.Constants;
using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class WeaponServiceTests
    {
        private IRepository repository;
        private IWeaponService weaponService;
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
            weaponService = new WeaponService(repository);

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
                    PlayerId = 2
                },

            });
            await repository.SaveChangesAsync();
        }





        [Test]
        public async Task Test_ClothingExistsAsync_ReturnsTrueWhenAmmoBoxExists()
        {
            var result = await weaponService.WeaponExistsAsync(1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_ClothingExistsAsync_ReturnsFalseWhenClothingDoesNotExist()
        {
            var result = await weaponService.WeaponExistsAsync(30);
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public async Task Test_UserCanBuyWeaponAsync_ReturnsTrueWhenUserCanBuyWeapon()
        {
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
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666"
            });
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
                new Weapon()
                {
                    Id = 2,
                    Name = "Glock 17",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    VendorId = 1
                },
            });
            await repository.SaveChangesAsync();

            var result = await weaponService.UserCanBuyWeaponAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserCanBuyWeaponAsync_ReturnsFalseWhenUserCannotBuyWeapon()
        {
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
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = false
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
               new Weapon()
                {
                    Id = 2,
                    Name = "Glock 17",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    VendorId = 1
                },
            });
            await repository.SaveChangesAsync();

            var result = await weaponService.UserCanBuyWeaponAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_UserCanSellWeaponAsync_ReturnsTrueWhenUserCanSellWeapon()
        {
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
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = true
            });
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
                 new Weapon()
                {
                    Id = 2,
                    Name = "Glock 17",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    PlayerId = 2
                },
            });
            await repository.SaveChangesAsync();

            var result = await weaponService.UserCanSellWeaponAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserCanSellWeaponAsync_ReturnsFalseWhenUserCannotSellWeapon()
        {
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
                }
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                IsActive = true
            });
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
                 new Weapon()
                {
                    Id = 2,
                    Name = "Glock 17",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    VendorId = 1
                },
            });
            await repository.SaveChangesAsync();

            var result = await weaponService.UserCanSellWeaponAsync("77388c0c-698c-4df9-9ad9-cef29116b666", 2);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_IsWeaponForSaleAsync_ReturnsTrueWhenWeaponIsForSale()
        {
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
            {
                 new Weapon()
                {
                    Id = 2,
                    Name = "Glock 17",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    WeaponType = WeaponType.Pistol,
                    Price = 50,
                    VendorId = 1
                },
            });
            await repository.SaveChangesAsync();
            var result = await weaponService.WeaponIsForSaleAsync(2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_IsWeaponForSaleAsync_ReturnsFalseWhenWeaponIsNotForSale()
        {

            var result = await weaponService.WeaponIsForSaleAsync(1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_GetClothingListModelForBuyAsync_ReturnsModelCorrectly()
        {
            var resultModel = await weaponService.GetWeaponListModelForBuyAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Glock 18"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.WeaponType, Is.EqualTo(WeaponType.Pistol));
            Assert.That(resultModel.Price, Is.EqualTo(50));
        }

        [Test]
        public async Task Test_CreateClothingSellModelAsync_ReturnsModelCorrectly()
        {
            var resultModel = await weaponService.CreateWeaponSellModelAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Glock 18"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.WeaponType, Is.EqualTo(WeaponType.Pistol));
            Assert.That(resultModel.Price, Is.EqualTo(50));
            Assert.That(resultModel.OldPrice, Is.EqualTo(50));
        }

        [Test]
        public async Task Test_GetClothingByIdAsync_ReturnsCorrectModel()
        {
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
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = true
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 2,
                Ammo = 200,
                UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await repository.SaveChangesAsync();
            var gameModel = await weaponService.GetWeaponByIdAsync(1);
            Assert.That(gameModel.Id, Is.EqualTo(1));
            Assert.That(gameModel.PlayerId, Is.EqualTo(2));
            Assert.That(gameModel.PlayerName, Is.EqualTo("Vendor"));
        }

        [Test]
        public async Task Test_CreateWeaponCreateModelByWeaponType_CreatesCorrectModel()
        {
            var model = weaponService.CreateWeaponCreateModelByWeaponType(WeaponType.Pistol);

            Assert.That(model.MaxFireRate, Is.EqualTo(WeaponConstants.PistolMaxFireRate));
            Assert.That(model.MinFireRate, Is.EqualTo(WeaponConstants.PistolMinFireRate));
            Assert.That(model.MinAverageAmmoExpended, Is.EqualTo(WeaponConstants.PistolMinAverageAmmoExpended));
            Assert.That(model.MaxAverageAmmoExpended, Is.EqualTo(WeaponConstants.PistolMaxAverageAmmoExpended));
            Assert.That(model.MinFireRate, Is.EqualTo(WeaponConstants.PistolMinFireRate));
            Assert.That(model.MaxFireRate, Is.EqualTo(WeaponConstants.PistolMaxFireRate));
            Assert.That(model.MaxFeetPerSecond, Is.EqualTo(WeaponConstants.PistolMaxFeetPerSecond));
            Assert.That(model.MinFeetPerSecond, Is.EqualTo(WeaponConstants.PistolMinFeetPerSecond));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
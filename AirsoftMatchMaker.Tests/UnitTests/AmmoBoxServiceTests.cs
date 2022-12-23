using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class AmmoBoxServiceTests
    {
        private IRepository repository;
        private IAmmoBoxService ammoBoxService;
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
            ammoBoxService = new AmmoBoxService(repository);

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
            });
            await repository.SaveChangesAsync();
        }



        [Test]
        public async Task Test_GetAmmoBoxByIdAsync_ReturnsNullWhenIdDoesNotExist()
        {
            await repository.SaveChangesAsync();
            var gameModel = await ammoBoxService.GetAmmoBoxByIdAsync(30);
            Assert.That(gameModel, Is.EqualTo(null));
        }

        [Test]
        public async Task Test_AmmoBoxExistsAsync_ReturnsTrueWhenAmmoBoxExists()
        {
            var result = await ammoBoxService.AmmoBoxExistsAsync(1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_AmmoBoxExistsAsync_ReturnsFalseWhenAmmoBoxDoesNotExist()
        {
            var result = await ammoBoxService.AmmoBoxExistsAsync(30);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_UserCanBuyAmmoBoxAsync_ReturnsTrueWhenUserCanBuyBox()
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

                Id = "77388c0c-698c-4df9-9ad9-cef29116b666\"",
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
            await repository.SaveChangesAsync();

            var result = await ammoBoxService.UserCanBuyAmmoBoxAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserCanBuyAmmoBoxAsync_ReturnsFalseWhenUserCannotBuyBox()
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
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await repository.AddAsync<Vendor>(new Vendor()
            {
                Id = 1,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2"
            });
            await repository.SaveChangesAsync();

            var result = await ammoBoxService.UserCanBuyAmmoBoxAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public async Task Test_UserHasEnoughCreditsAsync_ReturnsTrueWhenUserHasEnoughCredits()
        {
            await repository.AddAsync<User>(new User()
            {
                Id = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                UserName = "Ivan",
                NormalizedUserName = "IVAN",
                Email = "Ivan@gmail.com",
                NormalizedEmail = "IVAN@GMAIL.COM",
                Credits = 200
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1
            });
            await repository.AddAsync<Team>(new Team()
            {
                Id = 1,
                Name = "Test1",
            });
            await repository.SaveChangesAsync();
            var result = await ammoBoxService.UserHasEnoughCreditsAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1, 2);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task Test_UserHasEnoughCreditsAsync_ReturnsFalseWhenUserDoesNotHaveEnoughCredits()
        {
            await repository.AddAsync<User>(new User()
            {
                Id = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                UserName = "Ivan",
                NormalizedUserName = "IVAN",
                Email = "Ivan@gmail.com",
                NormalizedEmail = "IVAN@GMAIL.COM",
                Credits = 20
            });
            await repository.AddAsync<Player>(new Player()
            {
                Id = 1,
                Ammo = 200,
                UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                TeamId = 1,
                PlayerClassId = 1
            });
            await repository.AddAsync<Team>(new Team()
            {
                Id = 1,
                Name = "Test1",
            });
            await repository.SaveChangesAsync();
            var result = await ammoBoxService.UserHasEnoughCreditsAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1, 3);
            Assert.That(result, Is.EqualTo(false));
        }



        [Test]
        public async Task Test_GetAmmoBoxToBuyByIdAsync_ReturnsModelCorrectly()
        {
            var resultModel = await ammoBoxService.GetAmmoBoxToBuyByIdAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Small box"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.VendorId, Is.EqualTo(1));
            Assert.That(resultModel.AmmoAmount, Is.EqualTo(50));
            Assert.That(resultModel.Quantity, Is.EqualTo(900));
            Assert.That(resultModel.Price, Is.EqualTo(10));
        }
        [Test]
        public async Task Test_GetAmmoBoxForRestockByIdAsync_ReturnsModelCorrectly()
        {
            var resultModel = await ammoBoxService.GetAmmoBoxForRestockByIdAsync(1);
            Assert.That(resultModel.Name, Is.EqualTo("Small box"));
            Assert.That(resultModel.Id, Is.EqualTo(1));
            Assert.That(resultModel.VendorId, Is.EqualTo(1));
            Assert.That(resultModel.AmmoAmount, Is.EqualTo(50));
            Assert.That(resultModel.Quantity, Is.EqualTo(900));
            Assert.That(resultModel.Price, Is.EqualTo(10));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
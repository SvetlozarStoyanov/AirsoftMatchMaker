using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AirsoftMatchMaker.Tests.IntegrationTests
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
            await repository.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetAllAmmoBoxesAsync_WorksCorrectly()
        {
            var result = await ammoBoxService.GetAllAmmoBoxesAsync(
                null,
                AmmoBoxSorting.Newest,
                6,
                1
                );
            Assert.That(result.AmmoBoxes.Count(), Is.EqualTo(2));
            Assert.That(result.AmmoBoxes.ElementAt(0).Id, Is.EqualTo(2));
            Assert.That(result.AmmoBoxes.ElementAt(1).Id, Is.EqualTo(1));
            result = await ammoBoxService.GetAllAmmoBoxesAsync(
              "Small",
              AmmoBoxSorting.Newest,
              6,
              1
              );
            Assert.That(result.AmmoBoxes.Count(), Is.EqualTo(1));
            Assert.That(result.AmmoBoxes.ElementAt(0).Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_BuyAmmoBoxAsync_WorksCorrectly()
        {
            var player = await repository.All<Player>()
                .Where(p => p.Id == 1)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            var vendor = await repository.All<Vendor>()
                .Where(v => v.Id == 1)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            var ammoBox = await repository.GetByIdAsync<AmmoBox>(1);
            var model = new AmmoBoxBuyModel()
            {
                Id = ammoBox.Id,
                AmmoAmount = ammoBox.Amount,
                QuantityToBuy = 1,
                VendorId = vendor.Id,
                Price = ammoBox.Price
            };
            await ammoBoxService.BuyAmmoBoxAsync(player.UserId, model);
            Assert.That(player.Ammo, Is.EqualTo(250));
            Assert.That(player.User.Credits, Is.EqualTo(190));


        }


        [Test]
        public async Task Test_CreateAmmoBoxAsync_WorksCorrectly()
        {
            var vendor = await repository.All<Vendor>()
               .Where(v => v.Id == 1)
               .Include(v => v.User)
               .FirstOrDefaultAsync();
            var model = new AmmoBoxCreateModel()
            {
                Name = "TestBox",
                AmmoAmount = 100,
                Price = 50,
                Quantity = 2,
                FinalImportPrice = 50
            };
            await ammoBoxService.CreateAmmoBoxAsync(vendor.UserId, model);

            Assert.That(vendor.User.Credits, Is.EqualTo(150));
            var ammoBoxes = await repository.AllReadOnly<AmmoBox>().ToListAsync();
            Assert.That(ammoBoxes.Count, Is.EqualTo(3));
            Assert.That(ammoBoxes.Any(ab => ab.Id == 3), Is.EqualTo(true));
        }

        [Test]
        public async Task Test_RestockAmmoBoxAsync_WorksCorrectly()
        {
            var vendor = await repository.All<Vendor>()
               .Where(v => v.Id == 1)
               .Include(v => v.User)
               .FirstOrDefaultAsync();
            var model = new AmmoBoxRestockModel()
            {
                Id = 1,
                QuantityAdded = 2,
                FinalImportPrice = 10
            };
            await ammoBoxService.RestockAmmoBox(vendor.UserId, model);
            var ammoBox = await repository.GetByIdAsync<AmmoBox>(1);
            Assert.That(vendor.User.Credits, Is.EqualTo(190));
            Assert.That(ammoBox.Quantity, Is.EqualTo(902));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
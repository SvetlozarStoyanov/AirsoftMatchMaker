using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.IntegrationTests
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
            });
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
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetClothesAsync_WorksCorrectly()
        {
            var result = await clothingService.GetAllClothesAsync(
                null,
                ClothingSorting.Newest,
                null,
                6,
                1
                );
            Assert.That(result.Clothes.Count(), Is.EqualTo(2));
            Assert.That(result.Clothes.ElementAt(0).Id, Is.EqualTo(2));
            Assert.That(result.Clothes.ElementAt(1).Id, Is.EqualTo(1));
            result = await clothingService.GetAllClothesAsync(
                ClothingColor.Green,
                ClothingSorting.Oldest,
                null,
                6,
                1
                );
            Assert.That(result.Clothes.Count(), Is.EqualTo(1));
            Assert.That(result.Clothes.ElementAt(0).Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_BuyClothingAsync_WorksCorrectly()
        {
            var player = await unitOfWork.PlayerRepository.All()
                .Where(p => p.Id == 1)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            var vendor = await unitOfWork.VendorRepository.All()
                .Where(v => v.Id == 1)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            var clothing = await unitOfWork.ClothingRepository.GetByIdAsync(1);

            await clothingService.BuyClothingAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1);
            Assert.That(player.Clothes.Count, Is.EqualTo(1));
            Assert.That(player.User.Credits, Is.EqualTo(190));


        }


        [Test]
        public async Task Test_CreateClothingAsync_WorksCorrectly()
        {
            var vendor = await unitOfWork.VendorRepository.All()
               .Where(v => v.Id == 1)
               .Include(v => v.User)
               .FirstOrDefaultAsync();
            var model = new ClothingCreateModel()
            {
                Name = "Test Clothing",
                Description = "Hard to spot in forests",
                Price = 50,
                ClothingColor = ClothingColor.Green,
                FinalImportPrice = 25
            };
            await clothingService.CreateClothingAsync(vendor.UserId, model);

            Assert.That(vendor.User.Credits, Is.EqualTo(175));
            var clothes = await unitOfWork.ClothingRepository.AllReadOnly().ToListAsync();
            Assert.That(clothes.Count, Is.EqualTo(3));
            Assert.That(clothes.Any(ab => ab.Id == 3), Is.EqualTo(true));
        }

        [Test]
        public async Task Test_SellClothingAsync_WorksCorrectly()
        {
            await unitOfWork.UserRepository.AddAsync(new User
            {
                Id = "83086f19-cdb8-4b88-aafa-bc567172ac21",
                UserName = "Vendor",
                NormalizedUserName = "VENDOR",
                Email = "Vendor@gmail.com",
                NormalizedEmail = "VENDOR@GMAIL.COM",
                Credits = 200
            });
            await unitOfWork.PlayerRepository.AddAsync(new Player()
            {
                Id = 3,
                Ammo = 200,
                UserId = "83086f19-cdb8-4b88-aafa-bc567172ac21",
                TeamId = 1,
                PlayerClassId = 1,
                IsActive = false
            });
            await unitOfWork.VendorRepository.AddAsync(new Vendor()
            {
                Id = 2,
                UserId = "83086f19-cdb8-4b88-aafa-bc567172ac21",
                IsActive = true
            });


            await unitOfWork.ClothingRepository.AddAsync(new Clothing()
            {
                Id = 3,
                Name = "reen outfit",
                Description = "Hard to spot in forests",
                ClothingColor = ClothingColor.Green,
                Price = 10,
                PlayerId = 3
            });
            await unitOfWork.SaveChangesAsync();

            var model = new ClothingSellModel()
            {
                Id = 3,
                Name = "Green outfit",
                Description = "Hard to spot in forests",
                ClothingColor = ClothingColor.Green,
                Price = 8,
                OldPrice = 10,
            };
            var vendor = await unitOfWork.VendorRepository.All()
               .Where(v => v.Id == 2)
               .Include(v => v.User)
               .FirstOrDefaultAsync();
            var clothing = await unitOfWork.ClothingRepository.GetByIdAsync(3);
            Assert.That(clothing.PlayerId, Is.EqualTo(3));
            Assert.That(clothing.Price, Is.EqualTo(10));
            Assert.That(clothing.VendorId, Is.EqualTo(null));

            await clothingService.SellClothingAsync(vendor.UserId, model);
            Assert.That(clothing.PlayerId, Is.EqualTo(null));
            Assert.That(clothing.VendorId, Is.EqualTo(2));
            Assert.That(clothing.Price, Is.EqualTo(8));

        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
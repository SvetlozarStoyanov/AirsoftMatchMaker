using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class WeaponServiceTests
    {
        private IUnitOfWork unitOfWork;
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

            unitOfWork = new UnitOfWork(context);
            weaponService = new WeaponService(unitOfWork);

            await unitOfWork.WeaponRepository.AddRangeAsync(new List<Weapon>()
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
        public async Task Test_GetWeaponsAsync_WorksCorrectly()
        {
            var result = await weaponService.GetAllWeaponsAsync(
                null,
                null,
                WeaponSorting.Newest,
                null,
                6,
                1
                );
            Assert.That(result.Weapons.Count(), Is.EqualTo(2));
            Assert.That(result.Weapons.ElementAt(0).Id, Is.EqualTo(2));
            Assert.That(result.Weapons.ElementAt(1).Id, Is.EqualTo(1));
            result = await weaponService.GetAllWeaponsAsync(
                WeaponType.Pistol,
                null,
                WeaponSorting.Newest,
                null,
                6,
                1
                );
            Assert.That(result.Weapons.Count(), Is.EqualTo(1));
            Assert.That(result.Weapons.ElementAt(0).Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_BuyWeaponAsync_WorksCorrectly()
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

            await weaponService.BuyWeaponAsync("202efe8b-7748-49ca-834c-fd1c37978ab2", 1);
            Assert.That(player.Weapons.Count, Is.EqualTo(1));
            Assert.That(player.User.Credits, Is.EqualTo(150));


        }


        [Test]
        public async Task Test_CreateWeaponAsync_WorksCorrectly()
        {
            var vendor = await unitOfWork.VendorRepository.All()
               .Where(v => v.Id == 1)
               .Include(v => v.User)
               .FirstOrDefaultAsync();

            var model = new WeaponCreateModel()
            {
                Name = "Test Weapon",
                Description = "Test desc",
                Price = 50,
                WeaponType = WeaponType.Pistol,
                FinalImportPrice = 25
            };

            await weaponService.CreateWeaponAsync(vendor.UserId, model);

            Assert.That(vendor.User.Credits, Is.EqualTo(175));
            var weapons = await unitOfWork.WeaponRepository.AllReadOnly().ToListAsync();
            Assert.That(weapons.Count, Is.EqualTo(3));
            Assert.That(weapons.Any(w => w.Id == 3), Is.EqualTo(true));
        }

        [Test]
        public async Task Test_SellWeaponAsync_WorksCorrectly()
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


            await unitOfWork.WeaponRepository.AddAsync(new Weapon()
            {
                Id = 3,
                Name = "Test Weapon",
                Description = "Test desc",
                WeaponType = WeaponType.Pistol,
                Price = 10,
                PlayerId = 3
            });
            await unitOfWork.SaveChangesAsync();

            var model = new WeaponSellModel()
            {
                Id = 3,
                Name = "Test Weapon",
                Description = "Test desc",
                WeaponType = WeaponType.Pistol,
                Price = 8,
                OldPrice = 10,
            };
            var vendor = await unitOfWork.VendorRepository.All()
               .Where(v => v.Id == 2)
               .Include(v => v.User)
               .FirstOrDefaultAsync();
            var weapon = await unitOfWork.WeaponRepository.GetByIdAsync(3);
            Assert.That(weapon.PlayerId, Is.EqualTo(3));
            Assert.That(weapon.Price, Is.EqualTo(10));
            Assert.That(weapon.VendorId, Is.EqualTo(null));

            await weaponService.SellWeaponAsync(vendor.UserId, model);
            Assert.That(weapon.PlayerId, Is.EqualTo(null));
            Assert.That(weapon.VendorId, Is.EqualTo(2));
            Assert.That(weapon.Price, Is.EqualTo(8));

        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
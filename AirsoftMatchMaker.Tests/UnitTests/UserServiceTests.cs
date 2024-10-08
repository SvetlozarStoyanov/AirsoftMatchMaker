using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class UserServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IUserService userService;
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
            userService = new UserService(unitOfWork);

            await unitOfWork.UserRepository.AddAsync(new User()
            {

                Id = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                UserName = "Ivan",
                NormalizedUserName = "IVAN",
                Email = "Ivan@gmail.com",
                NormalizedEmail = "IVAN@GMAIL.COM",
                Credits = 190
            });
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_GetUserCreditsAsync_ReturnsCorrectCredits()
        {
            var result = await userService.GetUserCreditsAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            Assert.That(result, Is.EqualTo(190));
        }

        [Test]
        public async Task Test_GetCurrentUserProfileAsync_ReturnsCorrectModel()
        {
            var result = await userService.GetCurrentUserProfileAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            var user = await unitOfWork.UserRepository.GetByIdAsync("202efe8b-7748-49ca-834c-fd1c37978ab2");
            Assert.That(result.UserName, Is.EqualTo(user.UserName));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Credits, Is.EqualTo(user.Credits));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}

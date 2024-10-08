using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    internal class BackgroundGameServiceTests
    {
        private AirsoftMatchmakerDbContext context;
        private IUnitOfWork unitOfWork;
        private BackgroundGameService backgroundGameService;
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
            backgroundGameService = new BackgroundGameService(unitOfWork);

            await unitOfWork.UserRepository.AddRangeAsync(new List<User>()
            {
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Matchmaker",
                    NormalizedUserName = "MATCHMAKER",
                    Email = $"Matchmaker@gmail.com",
                    NormalizedEmail = $"MATCHMAKER@GMAIL.COM",
                },
                new User()
                {
                    Id = "0274ded9-003a-48eb-b608-7477597ec876",
                    UserName = "Player1",
                    NormalizedUserName = "PLAYER1",
                    Email = $"Player1@gmail.com",
                    NormalizedEmail = $"PLAYER1@GMAIL.COM",
                },
                new User()
                {
                    Id = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    UserName = "Player2",
                    NormalizedUserName = "PLAYER2",
                    Email = $"Player2@gmail.com",
                    NormalizedEmail = $"PLAYER2@GMAIL.COM",
                },
            });

            await unitOfWork.TeamRepository.AddRangeAsync(new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    Name = "Test1"
                },
                new Team()
                {
                    Id = 2,
                    Name = "Test2"
                },
            });
            await unitOfWork.GameModeRepository.AddAsync(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                PointsToWin = 3,
                Description = "",

            });
            await unitOfWork.MapRepository.AddAsync(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

            });
            await unitOfWork.MatchmakerRepository.AddAsync(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await unitOfWork.PlayerRepository.AddRangeAsync(new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    UserId = "0274ded9-003a-48eb-b608-7477597ec876",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 1,
                    IsActive = true,
                    SkillPoints = 200,
                },
                new Player()
                {
                    Id = 2,
                    UserId = "0b532089-f327-4dfa-a718-bc8bf8bad9a5",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 2,
                    IsActive = true,
                    SkillPoints = 200,
                }
            });
            await unitOfWork.PlayerClassRepository.AddRangeAsync(new List<PlayerClass>()
            {
                new PlayerClass()
                {
                    Id = 1,
                    Name = "New Player",
                    Description = "New to the game. Prone to make mistakes."
                }
            });
            await unitOfWork.WeaponRepository.AddRangeAsync(new List<Weapon>()
            {
                new Weapon()
                {
                    Id = 1,
                    Name = "Weapon1",
                    FeetPerSecond = 1000,
                    FireRate = 1000,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    AverageAmmoExpendedPerGame = 1000,
                    Description = "",
                    PlayerId = 1,
                    Price = 20,
                    VendorId = null,
                    WeaponType = WeaponType.Heavy
                },
                 new Weapon()
                {
                    Id = 2,
                    Name = "Weapon2",
                    FeetPerSecond= 1000,
                    FireRate= 1000,
                    PreferedEngagementDistance = PreferedEngagementDistance.Short,
                    AverageAmmoExpendedPerGame = 1000,
                    Description = "",
                    PlayerId = 2,
                    Price = 20,
                    VendorId = null,
                    WeaponType = WeaponType.Heavy
                }
            });
            await unitOfWork.GameRepository.AddRangeAsync(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = DateTime.Today.AddHours(-7),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                },
                new Game()
                {
                    Id = 2,
                    Name = "Test3 VS Test4",
                    Date = DateTime.Now,
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Upcoming,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = false,
                }
            });
            await unitOfWork.SaveChangesAsync();
        }

        [Test]
        public async Task Test_CalculateBettingOddsAsync_UpdatesOdds()
        {
            await backgroundGameService.CalculateBettingOddsAsync(2);
            var game = await unitOfWork.GameRepository.GetByIdAsync(2);
            Assert.That(game.OddsAreUpdated, Is.EqualTo(true));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }

}

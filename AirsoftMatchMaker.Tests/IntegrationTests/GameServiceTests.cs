using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using AngleSharp.Text;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace AirsoftMatchMaker.Tests.IntegrationTests
{
    public class GameServiceTests
    {
        private IRepository repository;
        private IGameService gameService;
        private AirsoftMatchmakerDbContext context;

        [SetUp]
        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<AirsoftMatchmakerDbContext>()
                .UseInMemoryDatabase("AirsoftMatchMakerTestDB")
                .Options;
            context = new AirsoftMatchmakerDbContext(contextOptions);

            context.ApplyConfiguration = false;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repository = new Repository(context);
            gameService = new GameService(repository);
        }

        [Test]
        public async Task Test_GetGameByIdAsync_ReturnsGameWhenIdExists()
        {
            await repository.AddRangeAsync<User>(new List<User>()
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

            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddAsync<GameMode>(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                Description = "",

            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

            });
            await repository.AddAsync<Matchmaker>(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await repository.AddRangeAsync<Player>(new List<Player>()
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
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });
            await repository.SaveChangesAsync();
            var gameModel = await gameService.GetGameByIdAsync(1);
            var expectedResult = new GameViewModel()
            {
                Id = 1,
                Name = "Test1 VS Test2",
                GameStatus = GameStatus.Upcoming,
                Date = new DateTime(2022, 2, 12).ToShortDateString(),
                EntryFee = 40,

                Map = new MapWithGameModeModel()
                {
                    Id = 1,
                    Name = "TestMap",
                    ImageUrl = null,
                    Terrain = TerrainType.Forest,
                    GameModeId = 1,
                    GameModeName = "TestMode"
                },
                MatchmakerId = 1,
                MatchmakerName = "Matchmaker",
                TeamRed = new TeamMinModel()
                {
                    Id = 1,
                    Name = "Test1",
                    Wins = 0,
                    Losses = 0,
                    Odds = "-126"
                },
                TeamBlue = new TeamMinModel()
                {
                    Id = 2,
                    Name = "Test2",
                    Wins = 0,
                    Losses = 0,
                    Odds = "+124"
                },
                IsAcceptingBets = false,
                Result = null
            };
            Assert.That(expectedResult.Id, Is.EqualTo(gameModel.Id));
            Assert.That(expectedResult.TeamRed.Id, Is.EqualTo(gameModel.TeamRed.Id));
            Assert.That(expectedResult.TeamBlue.Id, Is.EqualTo(gameModel.TeamBlue.Id));
            Assert.That(expectedResult.Map.Id, Is.EqualTo(gameModel.Map.Id));
            Assert.That(expectedResult.Map.GameModeId, Is.EqualTo(gameModel.Map.GameModeId));
        }

        [Test]
        public async Task Test_GetAllGamesAsync_ReturnsCorrectGames()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddRangeAsync<GameMode>(new List<GameMode>()
                {
                    new GameMode()
                    {
                        Id = 1,
                        Name = "TestMode1",
                        Description = "",
                    },
                    new GameMode()
                    {
                        Id = 2,
                        Name = "TestMode2",
                        Description = "",
                    },
            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1
            });
            await repository.AddRangeAsync<GameBetCreditsContainer>(new List<GameBetCreditsContainer>()
            {
                new GameBetCreditsContainer()
                {
                    Id = 1,
                    GameId = 1,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 2,
                    GameId = 2,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 3,
                    GameId = 3,
                    BetsArePaidOut = false,
                },
            });
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                    Result = "3:2"
                },

                new Game()
                {
                    Id = 2,
                    Name = "Test2 VS Test1",
                    Date = new DateTime(2022,2,14),
                    GameStatus = GameStatus.Finished,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 2,
                    TeamBlueId = 1,
                    TeamRedOdds = +130,
                    TeamBlueOdds = -130,
                    GameBetCreditsContainerId = 2,
                    OddsAreUpdated = true,
                    Result = "1:3"

                },
                new Game()
                {
                    Id = 3,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,15),
                    GameStatus = GameStatus.Upcoming,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -140,
                    TeamBlueOdds = +140,
                    GameBetCreditsContainerId = 3,
                    OddsAreUpdated = true,
                }
            });

            await repository.SaveChangesAsync();

            var model = await gameService.GetAllGamesAsync(
                   null,
                   "TestMode1",
                   null,
                   GameSorting.Newest,
                   6,
                   1);

            var ids = model.Games.Select(g => g.Id).ToList();
            Assert.That(model.Games.Count(), Is.EqualTo(3));
            Assert.That(ids, Is.EqualTo(new List<int>() { 3, 2, 1 }));

            model = await gameService.GetAllGamesAsync(
                   null,
                   "TestMode1",
                   null,
                   GameSorting.Oldest,
                   6,
                   1);

            ids = model.Games.Select(g => g.Id).ToList();

            Assert.That(model.Games.Count(), Is.EqualTo(3));
            Assert.That(ids, Is.EqualTo(new List<int>() { 1, 2, 3 }));
            model = await gameService.GetAllGamesAsync(
                   null,
                   "TestMode2",
                   null,
                   GameSorting.Newest,
                   6,
                   1);

            Assert.That(model.Games.Count(), Is.EqualTo(0));

            model = await gameService.GetAllGamesAsync(
                   null,
                   "TestMode1",
                   null,
                   GameSorting.Newest,
                   1,
                   1);

            Assert.That(model.Games.Count(), Is.EqualTo(1));

            model = await gameService.GetAllGamesAsync(
                  null,
                  null,
                  GameStatus.Upcoming,
                  GameSorting.Newest,
                  6,
                  1);
            Assert.That(model.Games.Count(), Is.EqualTo(1));

            model = await gameService.GetAllGamesAsync(
                  null,
                  null,
                  GameStatus.Finished,
                  GameSorting.Newest,
                  6,
                  1);
            Assert.That(model.Games.Count(), Is.EqualTo(2));
            model = await gameService.GetAllGamesAsync(
                 "TestTeam3",
                 "TestMode1",
                 null,
                 GameSorting.Newest,
                 6,
                 1);

            Assert.That(model.Games.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task Test_CreateGameAsync()
        {
            await repository.AddAsync<User>(

                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Matchmaker",
                    NormalizedUserName = "MATCHMAKER",
                    Email = $"Matchmaker@gmail.com",
                    NormalizedEmail = $"MATCHMAKER@GMAIL.COM",
                });

            await repository.AddAsync<Matchmaker>(
                new Matchmaker
                {
                    Id = 1,
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    IsActive = true,
                });
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddAsync<GameMode>(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                Description = "",

            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

            });

            var gameCreateModel = new GameCreateModel()
            {
                MapId = 1,
                EntryFee = 40,
                TeamRedId = 1,
                TeamBlueId = 2,
                DateString = DateTime.Now.AddDays(1).ToString()
            };


            await repository.SaveChangesAsync();

            await gameService.CreateGameAsync("c5d9e543-7c2f-4345-a014-ebd860eef718", gameCreateModel);

            var gamesCount = await repository.AllReadOnly<Game>()
                .CountAsync();

            Assert.That(gamesCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetUpcomingGamesByDateAsync()
        {
            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddRangeAsync<GameMode>(new List<GameMode>()
                {
                    new GameMode()
                    {
                        Id = 1,
                        Name = "TestMode1",
                        Description = "",
                    },
                    new GameMode()
                    {
                        Id = 2,
                        Name = "TestMode2",
                        Description = "",
                    },
            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1
            });
            await repository.AddRangeAsync<GameBetCreditsContainer>(new List<GameBetCreditsContainer>()
            {
                new GameBetCreditsContainer()
                {
                    Id = 1,
                    GameId = 1,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 2,
                    GameId = 2,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 3,
                    GameId = 3,
                    BetsArePaidOut = false,
                },
            });
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                    Result = "3:2"
                },

                new Game()
                {
                    Id = 2,
                    Name = "Test2 VS Test1",
                    Date = new DateTime(2022,2,14),
                    GameStatus = GameStatus.Finished,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 2,
                    TeamBlueId = 1,
                    TeamRedOdds = +130,
                    TeamBlueOdds = -130,
                    GameBetCreditsContainerId = 2,
                    OddsAreUpdated = true,
                    Result = "1:3"

                },
                new Game()
                {
                    Id = 3,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,15),
                    GameStatus = GameStatus.Upcoming,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -140,
                    TeamBlueOdds = +140,
                    GameBetCreditsContainerId = 3,
                    OddsAreUpdated = true,
                }
            });
            await repository.SaveChangesAsync();
            var games = await gameService.GetUpcomingGamesByDateAsync();
            Assert.That(games.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetPlayersLastFinishedAndFirstUpcomingGameAsync()
        {
            await repository.AddRangeAsync<User>(new List<User>()
            {
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
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Player3",
                    NormalizedUserName = "PLAYER3",
                    Email = $"Player3@gmail.com",
                    NormalizedEmail = $"PLAYER3@GMAIL.COM",
                },
                new User()
                {
                    Id = "e1423f2c-ce37-4a48-baa1-0143c66b8cc3",
                    UserName = "Player4",
                    NormalizedUserName = "PLAYER4",
                    Email = $"Player4@gmail.com",
                    NormalizedEmail = $"PLAYER4@GMAIL.COM",
                },
            });

            await repository.AddRangeAsync<Team>(new List<Team>()
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
                new Team()
                {
                    Id = 3,
                    Name = "Test3"
                },
            });
            await repository.AddAsync<GameMode>(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                Description = "",

            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

            });

            await repository.AddRangeAsync<Player>(new List<Player>()
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
                },
                 new Player()
                {
                    Id = 3,
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = null,
                    IsActive = true,
                    SkillPoints = 200,
                },
                new Player()
                {
                    Id = 4,
                    UserId = "e1423f2c-ce37-4a48-baa1-0143c66b8cc3",
                    Ammo = 200,
                    SkillLevel = SkillLevel.Beginner,
                    PlayerClassId = 1,
                    TeamId = 3,
                    IsActive = true,
                    SkillPoints = 200,
                }
            });
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                    new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                    Result = "3:2"
                },

                new Game()
                {
                    Id = 2,
                    Name = "Test2 VS Test1",
                    Date = new DateTime(2022,2,14),
                    GameStatus = GameStatus.Finished,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 2,
                    TeamBlueId = 1,
                    TeamRedOdds = +130,
                    TeamBlueOdds = -130,
                    GameBetCreditsContainerId = 2,
                    OddsAreUpdated = true,
                    Result = "1:3"

                },
                new Game()
                {
                    Id = 3,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,15),
                    GameStatus = GameStatus.Upcoming,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -140,
                    TeamBlueOdds = +140,
                    GameBetCreditsContainerId = 3,
                    OddsAreUpdated = true,
                }
            });
            await repository.SaveChangesAsync();
            var games = await gameService.GetPlayersLastFinishedAndFirstUpcomingGameAsync("0274ded9-003a-48eb-b608-7477597ec876");

            var ids = games.Select(g => g.Id).ToList();
            Assert.That(games.Count(), Is.EqualTo(2));
            Assert.That(ids, Is.EqualTo(new List<int> { 2, 3 }));

            games = await gameService.GetPlayersLastFinishedAndFirstUpcomingGameAsync("c5d9e543-7c2f-4345-a014-ebd860eef718");
            Assert.That(games.Count, Is.EqualTo(0));

            games = await gameService.GetPlayersLastFinishedAndFirstUpcomingGameAsync("e1423f2c-ce37-4a48-baa1-0143c66b8cc3");
            Assert.That(games.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Test_GetMatchmakersOrganisedGamesAsync_ReturnsCorrectGames()
        {
            await repository.AddRangeAsync<User>(new List<User>() {
                new User()
                {
                    Id = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    UserName = "Matchmaker1",
                    NormalizedUserName = "MATCHMAKER1",
                    Email = $"Matchmaker1@gmail.com",
                    NormalizedEmail = $"MATCHMAKER1@GMAIL.COM",
                },
                new User()
                {
                    Id = "f0165873-08dc-48b1-8379-01181d24a617",
                    UserName = "Matchmaker2",
                    NormalizedUserName = "MATCHMAKER2",
                    Email = $"Matchmaker2@gmail.com",
                    NormalizedEmail = $"MATCHMAKER2@GMAIL.COM",
                }
            });

            await repository.AddRangeAsync<Matchmaker>(new List<Matchmaker>()
            {
                new Matchmaker()
                {
                    Id = 1,
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    IsActive = true,
                },
                new Matchmaker()
                {
                    Id = 2,
                    UserId = "f0165873-08dc-48b1-8379-01181d24a617",
                    IsActive = true,
                }
            });


            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddRangeAsync<GameMode>(new List<GameMode>()
                {
                    new GameMode()
                {
                    Id = 1,
                    Name = "TestMode1",
                    Description = "",
                },
                new GameMode()
                {
                    Id = 2,
                    Name = "TestMode2",
                    Description = "",
                },
            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1
            });
            await repository.AddRangeAsync<GameBetCreditsContainer>(new List<GameBetCreditsContainer>()
            {
                new GameBetCreditsContainer()
                {
                    Id = 1,
                    GameId = 1,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 2,
                    GameId = 2,
                    BetsArePaidOut = true,
                },
                new GameBetCreditsContainer()
                {
                    Id = 3,
                    GameId = 3,
                    BetsArePaidOut = false,
                },
            });
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                    Result = "3:2"
                },

                new Game()
                {
                    Id = 2,
                    Name = "Test2 VS Test1",
                    Date = new DateTime(2022,2,14),
                    GameStatus = GameStatus.Finished,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 2,
                    TeamBlueId = 1,
                    TeamRedOdds = +130,
                    TeamBlueOdds = -130,
                    GameBetCreditsContainerId = 2,
                    OddsAreUpdated = true,
                    Result = "1:3"

                },
                new Game()
                {
                    Id = 3,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,15),
                    GameStatus = GameStatus.Upcoming,
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -140,
                    TeamBlueOdds = +140,
                    GameBetCreditsContainerId = 3,
                    OddsAreUpdated = true,
                }
            });

            await repository.SaveChangesAsync();
            var model = await gameService.GetMatchmakersOrganisedGamesAsync(
                1,
                MatchmakerGameStatus.Finalized,
                GameSorting.Newest,
                6,
                1);

            Assert.That(model.Games.Count(), Is.EqualTo(2));
            model = await gameService.GetMatchmakersOrganisedGamesAsync(
                1,
                MatchmakerGameStatus.Upcoming,
                GameSorting.Newest,
                6,
                1);

            Assert.That(model.Games.Count(), Is.EqualTo(1));
            model = await gameService.GetMatchmakersOrganisedGamesAsync(
               2,
               null,
               GameSorting.Newest,
               6,
               1);

            Assert.That(model.Games.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task Test_FinaliseGameAsync_FinalisesGameCorrectly()
        {
            await repository.AddRangeAsync<User>(new List<User>()
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

            await repository.AddRangeAsync<Team>(new List<Team>()
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
            await repository.AddAsync<GameMode>(new GameMode()
            {
                Id = 1,
                Name = "TestMode",
                PointsToWin = 3,
                Description = "",

            });
            await repository.AddAsync<Map>(new Map()
            {
                Id = 1,
                Name = "TestMap",
                ImageUrl = null,
                Description = "",
                Terrain = TerrainType.Forest,
                GameModeId = 1

            });
            await repository.AddAsync<Matchmaker>(new Matchmaker()
            {
                Id = 1,
                UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                IsActive = true,
            });
            await repository.AddRangeAsync<Player>(new List<Player>()
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
            await repository.AddRangeAsync<Weapon>(new List<Weapon>()
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
            await repository.AddRangeAsync<Game>(new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Test1 VS Test2",
                    Date = new DateTime(2022,2,12),
                    EntryFee = 40,
                    GameModeId = 1,
                    GameStatus = GameStatus.Finished,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                }
            });
            await repository.SaveChangesAsync();
            var model = new GameFinaliseModel()
            {
                Id = 1,
                GameModeMaxPoints = 3,
                TeamRedPoints = 3,
                TeamBluePoints = 2
            };
            await gameService.FinalizeGameAsync(model);
            var matchmaker = await repository.AllReadOnly<Matchmaker>()
                .Where(mm => mm.Id == 1)
                .Include(mm => mm.User)
                .FirstOrDefaultAsync();
            var game = await repository.GetByIdAsync<Game>(1);
            var player1 = await repository.AllReadOnly<Player>()
                .Where(p => p.Id == 1)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            var player2 = await repository.AllReadOnly<Player>()
                .Where(p => p.Id == 2)
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            Assert.That(matchmaker.User.Credits, Is.EqualTo(280));
            Assert.That(game.Result, Is.EqualTo($"3:2"));
            Assert.That(player1.User.Credits, Is.EqualTo(160));
            Assert.That(player2.User.Credits, Is.EqualTo(160));
        }
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
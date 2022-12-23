﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Services;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Tests.UnitTests
{
    public class PlayerServiceTests
    {
        private IRepository repository;
        private IPlayerService playerService;
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
            playerService = new PlayerService(repository);


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
            await repository.SaveChangesAsync();
        }


        [Test]
        public async Task Test_PlayerExistsAsync_ReturnsTrueWhenPlayerExists()
        {
            var result = await playerService.PlayerExistsAsync(1);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_PlayerExistsAsync_ReturnsFalseWhenPlayerDoesNotExist()
        {
            var result = await playerService.PlayerExistsAsync(35);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_GetPlayersTeamIdAsync_ReturnsCorrectId()
        {
            var result = await playerService.GetPlayersTeamIdAsync(1);
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public async Task Test_GetPlayersAvailableCreditsAsync_ReturnsCorrectId()
        {
            var result = await playerService.GetPlayersAvailableCreditsAsync("0274ded9-003a-48eb-b608-7477597ec876");
            Assert.That(result, Is.EqualTo(200));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}

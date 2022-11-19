using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasData(CreatePlayers());
        }
        private List<Player> CreatePlayers()
        {
            List<Player> players = new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    Ammo = 100,
                    UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    TeamId = 1,
                   
                    PlayerClassId = 1,
                    PlayerStatus = PlayerStatus.LookingToPlay
                },
                new Player()
                {
                    Id = 2,
                    Ammo = 100,
                    UserId = "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8",
                    TeamId = 1,
                    
                    PlayerClassId = 2,
                    PlayerStatus = PlayerStatus.LookingToPlay
                },
                new Player()
                {
                    Id = 3,
                    Ammo = 600,
                    UserId = "4d64daba-17d4-452c-af3e-5d731a250283",
                    TeamId = 2,
                   
                    PlayerClassId = 6,
                    PlayerStatus = PlayerStatus.LookingToPlay
                },
                new Player()
                {
                    Id = 4,
                    Ammo = 200,
                    UserId = "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    TeamId = 2,
                    
                    PlayerClassId = 7,
                    PlayerStatus = PlayerStatus.LookingToPlay
                },
                new Player()
                {
                    Id = 5,
                    Ammo = 450,
                    UserId = "f3534aed-259b-4ff7-b816-15e8207e084a",
                    TeamId = null,
                    
                    PlayerClassId = 3,
                    PlayerStatus = PlayerStatus.LookingForATeam
                },
                new Player()
                {
                    Id = 6,
                    Ammo = 450,
                    UserId = "f580c1f9-d41f-455e-b4ec-705b834e4b19",
                    TeamId = null,
                    
                    PlayerClassId = 3,
                    PlayerStatus = PlayerStatus.LookingForATeam
                },

                //new Player()
                //{
                //    Id = 7,
                //    Ammo = 350,
                //    UserId = "",
                //    TeamId = null,
                //    ClothingId = 3,
                //    PlayerClassId = 5,
                //},
                //new Player()
                //{
                //    Id = 8,
                //    Ammo = 1000,
                //    UserId = "",
                //    TeamId = 3,
                //    ClothingId = 2,
                //    PlayerClassId = 2,
                //},
                //new Player()
                //{
                //    Id = 9,
                //    Ammo = 1000,
                //    UserId = "",
                //    TeamId = 4,
                //    ClothingId = 1,
                //    PlayerClassId = 6,
                //},
                //new Player()
                //{
                //    Id = 10,
                //    Ammo = 1000,
                //    UserId = "",
                //    TeamId = 4,
                //    ClothingId = 1,
                //    PlayerClassId = 6,
                //},
            };
            return players;
        }
    }
}

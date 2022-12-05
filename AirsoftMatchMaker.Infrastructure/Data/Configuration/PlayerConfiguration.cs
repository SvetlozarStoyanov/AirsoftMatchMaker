using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            int i = 1;
            List<Player> players = new List<Player>()
            {
                new Player()
                {
                    Id = i++,
                    Ammo = 200,
                    UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    TeamId = 1,
                    PlayerClassId = 1

                },
                new Player()
                {
                    Id = i++,
                    Ammo = 200,
                    UserId = "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8",
                    TeamId = 1,
                    PlayerClassId = 2
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 600,
                    UserId = "4d64daba-17d4-452c-af3e-5d731a250283",
                    TeamId = 2,
                    PlayerClassId = 6
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 200,
                    UserId = "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    TeamId = 2,
                    PlayerClassId = 7
                },
                //Seeded for team request demo
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "f3534aed-259b-4ff7-b816-15e8207e084a",
                    TeamId = null,
                    PlayerClassId = 1
                },
                //Seeded for team request demo
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "f580c1f9-d41f-455e-b4ec-705b834e4b19",
                    TeamId = null,
                    PlayerClassId = 4
                },

                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "14677dd9-7de7-41c0-9418-e43ddcf64859",
                    TeamId = 3,
                    PlayerClassId = 6
                },
                 new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "c95011ef-d0e4-49c0-bbdd-1b9985bf7a74",
                    TeamId = 3,
                    PlayerClassId = 7
                },
                  new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "1f1087d3-a55a-4b7a-932e-1c3f9817fcf0",
                    TeamId = 3,
                    PlayerClassId = 4
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "5f83ea0f-418b-463f-9a52-bf1b9eac8bc6",
                    TeamId = 3,
                    PlayerClassId = 5
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "0a9aab7f-739a-41d8-b18d-8b797c7a2dfe",
                    TeamId = 3,
                    PlayerClassId = 1
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "3bf3238b-ab04-4945-8bd0-1eabf8a208d5",
                    TeamId = 4,
                    PlayerClassId = 4
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "18a322e4-ade8-4f13-8981-4cac7be64b9c",
                    TeamId = 4,
                    PlayerClassId = 6
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "799495ef-8794-491d-94d9-6bd37d51ba40",
                    TeamId = 4,
                    PlayerClassId = 7
                },
                new Player()
                {
                    Id = i++,
                    Ammo = 450,
                    UserId = "6f4bc586-751a-4a4b-8fec-4c7145b47a3e",
                    TeamId = 4,
                    PlayerClassId = 1
                },


            };
            return players;
        }
    }
}

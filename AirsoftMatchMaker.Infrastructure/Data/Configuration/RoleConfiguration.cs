using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(CreateRoles());
        }
        private List<Role> CreateRoles()
        {
            string[] roleGuids =
            {
                "52f73adc-3c27-40de-b00e-2e2b382da84c",
                "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce",
                "6b3c10a1-4a55-411a-8dca-49574cb55e74",
                "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8",
                "b48af83e-7873-4ecd-82de-5d517e7b31f9"
            };
            int i = 0;
            List<Role> roles = new List<Role>()
            {
                new Role()
                {
                    Id = roleGuids[i],
                    ConcurrencyStamp = roleGuids[i++],
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Description = "Responsible for granting roles."
                },
                new Role()
                {
                    Id = roleGuids[i],
                    ConcurrencyStamp = roleGuids[i++],
                    Name = "Vendor",
                    NormalizedName = "VENDOR",
                    Description = "Imports and sells items. Cannot be a active player or matchmaker."
                },
                new Role()
                {
                    Id = roleGuids[i],
                    ConcurrencyStamp = roleGuids[i++],
                    Name = "Player",
                    NormalizedName = "PLAYER",
                    Description = "Participates in games. Cannot be a active vendor or matchmaker."

                },
                new Role()
                {
                    Id = roleGuids[i],
                    ConcurrencyStamp = roleGuids[i++],
                    Name = "Matchmaker",
                    NormalizedName = "MATCHMAKER",
                    Description = "Creates games and collects entry fee. Cannot be a active vendor or player. Cannot bet on games!"

                },
                new Role()
                {
                    Id = roleGuids[i],
                    ConcurrencyStamp = roleGuids[i++],
                    Name = "GuestUser",
                    NormalizedName = "GUESTUSER",
                    Description = "User with no other roles. Can only bet on games."
                },
            };
            return roles;
        }
    }
}

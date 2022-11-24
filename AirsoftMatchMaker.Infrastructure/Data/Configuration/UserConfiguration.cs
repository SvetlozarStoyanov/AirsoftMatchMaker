using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(CreateUsers());
        }
        private List<User> CreateUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();
            List<User> users = new List<User>();
            string[] names = { "Petar", "Georgi", "Ivan", "Michael", "Alexander", "Todor", "Hank", "Vasil", "Krum", "Joe","Paul" };
            string[] guids =
                {
                    //Admins
                    "56d661fd-2339-498a-bd7e-c95f37908b28",
                    //Players
                    "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8",
                    "4d64daba-17d4-452c-af3e-5d731a250283",
                    "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    "f3534aed-259b-4ff7-b816-15e8207e084a",
                    "f580c1f9-d41f-455e-b4ec-705b834e4b19",
                    //Vendors
                    "77388c0c-698c-4df9-9ad9-cef29116b666",
                    //Matchmakers
                    "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    //Guest users
                    "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                    "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61"

                };
            var password = "password";
            int i = 0;
            foreach (string name in names)
            {
                var user = new User()
                {
                    Id = guids[i++],
                    UserName = name,
                    NormalizedUserName = name.ToUpper(),
                    Email = $"{name}@gmail.com",
                    NormalizedEmail = $"{name.ToUpper()}@GMAIL.COM",
                };
                user.PasswordHash = hasher.HashPassword(user, password);
                users.Add(user);
            }
            return users;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(SetUsersToRoles());
        }
        private List<IdentityUserRole<string>> SetUsersToRoles()
        {
            List<IdentityUserRole<string>> usersAndRoles = new List<IdentityUserRole<string>>()
            {
                //Admins
                new IdentityUserRole<string>()
                {
                    UserId = "56d661fd-2339-498a-bd7e-c95f37908b28",
                    RoleId = "52f73adc-3c27-40de-b00e-2e2b382da84c"
                },
                //Players
                new IdentityUserRole<string>()
                {
                    UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "4d64daba-17d4-452c-af3e-5d731a250283",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "f3534aed-259b-4ff7-b816-15e8207e084a",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "f580c1f9-d41f-455e-b4ec-705b834e4b19",
                    RoleId = "6b3c10a1-4a55-411a-8dca-49574cb55e74"
                },
                //Vendors
                new IdentityUserRole<string>()
                {
                    UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                    RoleId = "d0bd950a-e2d5-46cf-a6c1-1f0efa4144ce"
                },
                //Matchmakers
                new IdentityUserRole<string>()
                {
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    RoleId = "fc9628b0-fa92-4be1-9f1f-9095d66f1ff8"
                },
                //GuestUsers
                new IdentityUserRole<string>()
                {
                    UserId = "56d661fd-2339-498a-bd7e-c95f37908b28",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "202efe8b-7748-49ca-834c-fd1c37978ab2",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "2a1bd8b6-6d06-470b-9dda-fe88aa1bf5e8",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "4d64daba-17d4-452c-af3e-5d731a250283",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "f3534aed-259b-4ff7-b816-15e8207e084a",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
                new IdentityUserRole<string>()
                {
                    UserId = "f580c1f9-d41f-455e-b4ec-705b834e4b19",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },

                new IdentityUserRole<string>()
                {
                    UserId = "77388c0c-698c-4df9-9ad9-cef29116b666",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },

                new IdentityUserRole<string>()
                {
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                    RoleId = "b48af83e-7873-4ecd-82de-5d517e7b31f9"
                },
            };
            return usersAndRoles;
        }

    }
}

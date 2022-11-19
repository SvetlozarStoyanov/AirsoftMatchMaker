using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class ClothingConfiguration : IEntityTypeConfiguration<Clothing>
    {
        private List<int> playerIds = new List<int>();
        public void Configure(EntityTypeBuilder<Clothing> builder)
        {
            builder.HasData(CreateClothes());
        }
        private List<Clothing> CreateClothes()
        {
            for (int i = 1; i <= 6; i++)
            {
                playerIds.Add(i);
            }

            List<Clothing> clothes = new List<Clothing>() {
                new Clothing()
                {
                    Id = 1,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon()

                },
                new Clothing()
                {
                    Id = 2,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 3,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),

                },
                new Clothing()
                {
                    Id = 4,
                    Name = "Green Ghillie Suit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 80,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = 5,
                    Name = "Green Ghillie Suit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 80,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 6,
                    Name = "Green Army camouflage",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 60,
                    VendorId = 1,
                },
                new Clothing()
                {
                    Id = 7,
                    Name = "Green Army camouflage",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 60,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 8,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 9,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Clothing()
                {
                    Id = 10,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    VendorId = 1
                },
                 new Clothing()
                {
                    Id = 11,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 12,
                    Name = "Gray tracksuit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 20,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 13,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Clothing()
                {
                    Id = 14,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 15,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 16,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 17,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 18,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Clothing()
                {
                    Id = 19,
                    Name = "White outfit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 40,
                    VendorId = 1,
                },
                new Clothing()
                {
                    Id = 20,
                    Name = "White outfit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 40,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 21,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1,
                },
                new Clothing()
                {
                    Id = 22,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = 23,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1
                },
            };
            return clothes;
        }
        private int AddPlayerIdToWeapon()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, playerIds.Count - 1);
            int playerId = playerIds[randomIndex];
            playerIds.RemoveAt(randomIndex);
            return playerId;
        }
    }
}

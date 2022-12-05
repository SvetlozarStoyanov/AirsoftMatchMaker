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
            for (int i = 1; i <= 15; i++)
            {
                playerIds.Add(i);
            }

            int index = 1;
            List<Clothing> clothes = new List<Clothing>() {
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),

                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),

                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),

                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green outfit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 50,
                    PlayerId = AddPlayerIdToWeapon(),

                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green Ghillie Suit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 80,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green Ghillie Suit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 80,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green Army camouflage",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 60,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green Army camouflage",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 60,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    PlayerId = AddPlayerIdToWeapon()
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                 new Clothing()
                {
                    Id = index++,
                    Name = "Urban outfit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 40,
                    PlayerId = AddPlayerIdToWeapon(),
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Gray tracksuit",
                    Description = "Hard to spot in urban enviroment.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Gray,
                    Price = 20,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Army outfit ",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 30,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Brown Ghillie suit",
                    Description = "Hard to spot in field.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Brown,
                    Price = 45,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "White outfit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 40,
                    VendorId = 1,
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "White outfit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 40,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1,
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "White Ghillie suit",
                    Description = "Hard to spot in snow.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.White,
                    Price = 55,
                    VendorId = 1
                },
                new Clothing()
                {
                    Id = index++,
                    Name = "Green Ghillie Suit",
                    Description = "Hard to spot in forest.",
                    ImageUrl = null,
                    ClothingColor = ClothingColor.Green,
                    Price = 80,
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

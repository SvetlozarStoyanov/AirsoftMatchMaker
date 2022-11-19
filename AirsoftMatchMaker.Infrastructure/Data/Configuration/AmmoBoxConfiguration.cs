using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class AmmoBoxConfiguration : IEntityTypeConfiguration<AmmoBox>
    {
        public void Configure(EntityTypeBuilder<AmmoBox> builder)
        {
            builder.HasData(CreateAmmoBoxes());
        }
        private List<AmmoBox> CreateAmmoBoxes()
        {
            List<AmmoBox> ammoBoxes = new List<AmmoBox>()
            {
                new AmmoBox()
                {
                    Id = 1,
                    Name = "Small box",
                    Amount = 50,
                    Price = 10,
                    Quantity = 900,
                    VendorId = 1
                },
                new AmmoBox()
                {
                    Id = 2,
                    Name = "Large box",
                    Amount = 150,
                    Price = 20,
                    Quantity = 700,
                    VendorId = 1
                },
                new AmmoBox()
                {
                    Id = 3,
                    Name = "Extra Large box",
                    Amount = 300,
                    Price = 40,
                    Quantity = 500,
                    VendorId = 1
                },
                new AmmoBox()
                {
                    Id = 4,
                    Name= "Needlessly Large box",
                    Amount = 1000,
                    Price = 100,
                    Quantity = 300,
                    VendorId = 1
                }
            };
            return ammoBoxes;
        }
    }
}

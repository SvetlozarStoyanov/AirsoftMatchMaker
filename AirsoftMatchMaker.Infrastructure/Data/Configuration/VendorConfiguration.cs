using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasData(CreateVendors());
        }
        private List<Vendor> CreateVendors()
        {
            List<Vendor> vendors = new List<Vendor>()
            {
                new Vendor()
                {
                    Id = 1,
                    UserId = "77388c0c-698c-4df9-9ad9-cef29116b666"
                }
            };
            return vendors;
        }
    }
}

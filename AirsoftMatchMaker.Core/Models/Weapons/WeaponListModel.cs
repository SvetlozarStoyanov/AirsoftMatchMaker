using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public WeaponType WeaponType { get; set; }
        public PreferedEngagementDistance PreferedEngagementDistance { get; set; }
        //public int? VendorId { get; set; }
        //public virtual Vendor? Vendor { get; set; }
        //public int? PlayerId { get; set; }
        //public virtual Player? Player { get; set; }
    }
}

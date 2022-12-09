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
    }
}

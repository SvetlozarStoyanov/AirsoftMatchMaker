using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponCreateModel
    {
        public WeaponCreateModel()
        {
            PreferedEngagementDistances = new HashSet<PreferedEngagementDistance>();
        }
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3), MaxLength(200)]
        public string Description { get; set; } = null!;
        public  double FeetPerSecond { get; init; }
        public  double FireRate { get; init; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int AverageAmmoExpendedPerGame { get; init; }
        public WeaponType WeaponType { get; init; }
        public PreferedEngagementDistance PreferedEngagementDistance { get; set; }
        public ICollection<PreferedEngagementDistance> PreferedEngagementDistances { get; set; }
    }
}

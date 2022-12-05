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
        [Range(1, 100, ErrorMessage = "Feet per second must be within given range!")]
        public double FeetPerSecond { get; set; }
        [Range(1, 100, ErrorMessage = "Fire rate must be within given range!")]
        public double FireRate { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        [Range(1, 100, ErrorMessage = "Average ammo expended must be within given range!")]
        public int AverageAmmoExpendedPerGame { get; set; }
        public WeaponType WeaponType { get; init; }
        public PreferedEngagementDistance PreferedEngagementDistance { get; set; }
        public ICollection<PreferedEngagementDistance> PreferedEngagementDistances { get; set; }
        public decimal FinalImportPrice { get; set; }

        public double MinFeetPerSecond { get; set; }
        public double MaxFeetPerSecond { get; set; }
        public double MinFireRate { get; set; }
        public double MaxFireRate { get; set; }
        public double MinAverageAmmoExpended { get; set; }
        public double MaxAverageAmmoExpended { get; set; }
    }
}

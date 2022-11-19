using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Weapon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        public decimal FeetPerSecond { get; set; }
        public decimal FireRate { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int AverageAmmoExpendedPerGame { get; set; }
        public WeaponType WeaponType { get; set; }
        public PreferedEngagementDistance PreferedEngagementDistance { get; set; }

        [ForeignKey(nameof(Entities.Vendor.Id))]
        public int? VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }

        [ForeignKey(nameof(Entities.Player.Id))]
        public int? PlayerId { get; set; }
        public virtual Player? Player { get; set; }
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
        public double FeetPerSecond { get; set; }
        public double FireRate { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int AverageAmmoExpendedPerGame { get; set; }
        public WeaponType WeaponType { get; set; }
        public PreferedEngagementDistance PreferedEngagementDistance { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public int? PlayerId { get; set; }
        public string? PlayerName { get; set; }
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponSellModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(3), MaxLength(200)]
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public WeaponType WeaponType { get; init; }
        public decimal OldPrice { get; set; }
    }
}

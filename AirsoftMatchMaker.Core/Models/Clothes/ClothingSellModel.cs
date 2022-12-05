using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothingSellModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3), MaxLength(60)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5), MaxLength(200)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public ClothingColor ClothingColor { get; set; }
        public decimal OldPrice { get; set; }
    }
}

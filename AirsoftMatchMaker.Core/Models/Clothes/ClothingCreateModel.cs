using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothingCreateModel
    {
        public ClothingCreateModel()
        {
            Colors = new HashSet<ClothingColor>();
        }
        [Required]
        [MinLength(3), MaxLength(60)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5), MaxLength(200)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        [Range(1, 100)]
        public decimal Price { get; set; }
        public ClothingColor ClothingColor { get; set; }
        public ICollection<ClothingColor> Colors { get; init; }
    }
}

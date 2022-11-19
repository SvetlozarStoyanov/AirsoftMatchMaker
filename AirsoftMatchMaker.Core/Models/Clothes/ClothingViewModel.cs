using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public ClothingColor ClothingColor { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public int? PlayerId { get; set; }
        public string? PlayerName { get; set; }
    }
}

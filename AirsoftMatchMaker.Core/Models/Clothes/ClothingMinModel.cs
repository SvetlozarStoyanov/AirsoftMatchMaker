using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothingMinModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? ImageUrl { get; init; }
        public decimal Price { get; init; }
        public ClothingColor ClothingColor { get; init; }
    }
}

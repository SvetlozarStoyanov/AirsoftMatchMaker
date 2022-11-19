using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
    }
}

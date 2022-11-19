using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothingListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ClothingColor ClothingColor { get; set; }
        //public int? VendorId { get; set; }
        //public virtual Vendor? Vendor { get; set; }
    }
}

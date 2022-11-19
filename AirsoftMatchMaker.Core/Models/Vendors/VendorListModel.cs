namespace AirsoftMatchMaker.Core.Models.Vendors
{
    public class VendorListModel
    {
        public int Id { get; init; }
        public string UserId { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public int AmmoBoxesForSaleCount { get; init; }
        public int ClothesForSaleCount { get; init; }
        public int WeaponsForSaleCount { get; init; }
    }
}

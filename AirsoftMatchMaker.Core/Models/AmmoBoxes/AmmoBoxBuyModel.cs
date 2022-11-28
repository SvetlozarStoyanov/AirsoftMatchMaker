namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxBuyModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AmmoAmount { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int QuantityToBuy { get; set; }
        public decimal FinalPrice { get; set; }
    }
}

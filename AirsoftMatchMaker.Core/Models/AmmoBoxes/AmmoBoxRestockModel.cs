namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxRestockModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public int AmmoAmount { get; set; }
        public decimal Price { get; set; }
        public int QuantityAdded { get; set; }
        public int VendorId { get; set; }
    }
}

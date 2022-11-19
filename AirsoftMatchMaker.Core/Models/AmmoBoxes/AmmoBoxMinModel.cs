namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxMinModel
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public int Amount { get; init; }
        public decimal Price { get; init; }
    }
}

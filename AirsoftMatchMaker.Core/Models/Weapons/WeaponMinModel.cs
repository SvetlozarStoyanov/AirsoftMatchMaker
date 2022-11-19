namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponMinModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}

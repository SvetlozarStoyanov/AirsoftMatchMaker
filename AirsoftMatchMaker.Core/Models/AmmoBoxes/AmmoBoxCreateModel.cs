using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxCreateModel
    {
        [Required]
        [MinLength(5), MaxLength(30)]
        public string Name { get; set; } = null!;
        [Display(Name = "Ammo amount")]
        [Range(50, 2000)]
        public int AmmoAmount { get; set; }
        [Range(1, 400)]
        public decimal Price { get; set; }
        [Range(1, 1000)]
        public int Quantity { get; set; }
        public decimal FinalImportPrice { get; set; }
    }
}

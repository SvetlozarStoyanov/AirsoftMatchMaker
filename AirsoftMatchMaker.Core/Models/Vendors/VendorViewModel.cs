using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Weapons;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Vendors
{
    public class VendorViewModel
    {
        public VendorViewModel()
        {
            AmmoBoxes = new HashSet<AmmoBoxMinModel>();
            Clothes = new HashSet<ClothingMinModel>();
            Weapons = new HashSet<WeaponMinModel>();
        }
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string UserId { get; init; } = null!;
        [Display(Name = "Username")]
        public string UserName { get; init; } = null!;
        public virtual ICollection<AmmoBoxMinModel> AmmoBoxes { get; init; }
        public virtual ICollection<ClothingMinModel> Clothes { get; init; }
        public virtual ICollection<WeaponMinModel> Weapons { get; init; }
    }
}

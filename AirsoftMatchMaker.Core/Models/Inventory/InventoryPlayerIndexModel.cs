using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Weapons;

namespace AirsoftMatchMaker.Core.Models.Inventory
{
    public class InventoryPlayerIndexModel
    {
        public InventoryPlayerIndexModel()
        {
            Clothes = new List<ClothingListModel>();
            Weapons = new List<WeaponListModel>();
        }
        public int Ammo { get; set; }
        public ICollection<ClothingListModel> Clothes { get; set; }
        public ICollection<WeaponListModel> Weapons { get; set; }

    }
}

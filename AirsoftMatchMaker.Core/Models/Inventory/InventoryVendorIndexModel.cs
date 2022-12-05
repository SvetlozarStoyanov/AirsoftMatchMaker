using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Weapons;

namespace AirsoftMatchMaker.Core.Models.Inventory
{
    public class InventoryVendorIndexModel
    {
        public InventoryVendorIndexModel()
        {

            AmmoBoxes = new List<AmmoBoxListModel>();
            Clothes = new List<ClothingListModel>();
            Weapons = new List<WeaponListModel>();
        }

        public ICollection<AmmoBoxListModel> AmmoBoxes { get; set; }
        public ICollection<ClothingListModel> Clothes { get; set; }
        public ICollection<WeaponListModel> Weapons { get; set; }
    }
}

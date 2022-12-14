using AirsoftMatchMaker.Infrastructure.Data.Entities;

using AirsoftMatchMaker.Core.Models.Inventory;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Weapons;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IInventoryService
    {

        /// <summary>
        /// Returns <see cref="InventoryPlayerIndexModel"/> which contains information about <see cref="Player.Ammo"/> 
        /// and collections of <see cref="WeaponListModel"/> and <see cref="ClothingListModel"/> which belong to the player
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="InventoryPlayerIndexModel"/></returns>
        Task<InventoryPlayerIndexModel> GetPlayerItemsAsync(string userId);

        /// <summary>
        /// Returns <see cref="InventoryVendorIndexModel"/> which contains collections of
        /// <see cref="AmmoBoxListModel"/>,<see cref="WeaponListModel"/> and <see cref="ClothingListModel"/> which belong to the vendor
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="InventoryVendorIndexModel"/></returns>
        Task<InventoryVendorIndexModel> GetVendorItemsAsync(string userId);
    }
}

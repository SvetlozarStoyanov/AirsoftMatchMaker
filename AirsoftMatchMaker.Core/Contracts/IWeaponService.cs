using AirsoftMatchMaker.Core.Models.Weapons;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IWeaponService
    {
        /// <summary>
        /// Gets all weapons from the dbContext
        /// </summary>
        /// <returns><see cref="IEnumerable{T}" /></returns>
        Task<IEnumerable<WeaponListModel>> GetAllWeaponsAsync();

        /// <summary>
        /// Returns Weapon with given Id or null if it does not exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="WeaponViewModel"/></returns>
        Task<WeaponViewModel> GetWeaponByIdAsync(int id);

        /// <summary>
        /// Adds weapon to inventory of buyer , subtracting the weapon's price from his credits and adding them to the seller's balance
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="vendorId"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        Task BuyWeaponAsync(string buyerId, int vendorId, int weaponId);
    }
}

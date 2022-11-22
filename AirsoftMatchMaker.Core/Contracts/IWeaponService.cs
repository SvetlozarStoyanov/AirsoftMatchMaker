using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

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
        /// Adds weapon to inventory of buyer, subtracting the weapon's price from his credits and adding them to the seller's balance
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="vendorId"></param>
        /// <param name="weaponId"></param>
        /// <returns></returns>
        Task BuyWeaponAsync(string buyerId, int vendorId, int weaponId);

        /// <summary>
        /// Creates appropriate <see cref="WeaponCreateModel model"/> according to the <see cref="WeaponType weaponType"/>
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns><see cref="WeaponCreateModel model"/></returns>
        WeaponCreateModel CreateWeaponCreateModelByWeaponType(WeaponType weaponType);

        /// <summary>
        /// Checks if the feet per second and fire rate requirements are met. If they are not it returns error messages in form of strings.
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        IEnumerable<string> ValidateWeaponParameters(WeaponCreateModel model);

        /// <summary>
        /// Creates a <see cref="Weapon weapon"/> from <see cref="WeaponCreateModel model"/>
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateWeaponAsync(string vendorUserId, WeaponCreateModel model);

    }
}

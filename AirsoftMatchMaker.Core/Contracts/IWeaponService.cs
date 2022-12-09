using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IWeaponService
    {
        /// <summary>
        /// Returns true if <see cref="Weapon"/> exists, false otherwise
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> WeaponExistsAsync(int id);

        /// <summary>
        /// Returns true if the user selling the <see cref="Weapon"/> is not the same
        /// as the user trying to buy it.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="weaponId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserCanBuyWeaponAsync(string userId, int weaponId);

        /// <summary>
        /// Returns true if the user trying to buy the <see cref="Weapon"/> is not the same
        /// as the user selling it.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="weaponId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserCanSellWeaponAsync(string userId, int weaponId);

        /// <summary>
        /// If  user does not have enough credits returns false 
        /// (if user is player his upcoming games entry fee is also calculated)
        /// ,otherwise returns true.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="weaponId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserHasEnoughCreditsAsync(string userId, int weaponId);

        Task<WeaponsQueryModel> GetAllWeaponsAsync(
            WeaponType? weaponType = null,
            PreferedEngagementDistance? range = null,
            WeaponSorting weaponSorting = WeaponSorting.PriceAscending,
            string? searchTerm = null,
            int weaponsPerPage = 6,
            int currentPage = 1
            );

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
        /// Creates a <see cref="Weapon weapon"/> from <see cref="WeaponCreateModel model"/>
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateWeaponAsync(string vendorUserId, WeaponCreateModel model);

        /// <summary>
        /// Creates a <see cref="WeaponSellModel"/> in order to sell a weapon.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="WeaponSellModel"/></returns>
        Task<WeaponSellModel> CreateWeaponSellModelAsync(int id);

        /// <summary>
        /// Removes <see cref="Weapon"/> from <see cref="Player"/> inventory and lists it for sale with given price.
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task SellWeaponAsync(string vendorUserId, WeaponSellModel model);
    }
}

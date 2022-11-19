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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="WeaponViewModel"/></returns>
        Task<WeaponViewModel> GetWeaponByIdAsync(int id);
    }
}

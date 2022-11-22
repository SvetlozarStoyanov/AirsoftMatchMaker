using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IAmmoBoxService
    {
        /// <summary>
        /// Returns all ammo boxes with quantity>0
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<AmmoBoxListModel>> GetAllAmmoBoxesAsync();

        /// <summary>
        /// Returns ammo box with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AmmoBoxViewModel"/></returns>
        Task<AmmoBoxViewModel> GetAmmoBoxByIdAsync(int id);

        /// <summary>
        /// Returns ammo box which will be bought with given id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AmmoBoxBuyModel"/></returns>
        Task<AmmoBoxBuyModel> GetAmmoBoxToBuyByIdAsync(int id);

        /// <summary>
        /// Player buys ammo. Reduces ammoBox quantity by <see cref="AmmoBoxBuyModel.AmmoAmount"/>. If quantity reaches 0 it does not delete from the database.
        /// </summary>
        /// <param name="playerUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task BuyAmmoBoxAsync(string playerUserId, AmmoBoxBuyModel model);

        /// <summary>
        /// Creates <see cref="AmmoBox"/> from given model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateAmmoBoxAsync(string vendorUserId, AmmoBoxCreateModel model);

        /// <summary>
        /// Returns ammoBox model with current Quantity and vendor Id. With the option of adding more quantity to it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AmmoBoxRestockModel> GetAmmoBoxForRestockByIdAsync(int id);

        /// <summary>
        /// Adds quantity to already existing <see cref="AmmoBox ammoBox"/>
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task RestockAmmoBox(string vendorUserId, AmmoBoxRestockModel model);
    }
}

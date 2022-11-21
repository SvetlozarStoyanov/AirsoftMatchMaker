using AirsoftMatchMaker.Core.Models.AmmoBoxes;

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
        /// Player buys ammo. Reduces ammoBox quantity by 1 . If quantity reaches 0 it does not delete from the database
        /// </summary>
        /// <param name="playerUserId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        Task BuyAmmoBoxAsync(string playerUserId, AmmoBoxBuyModel model);
    }
}

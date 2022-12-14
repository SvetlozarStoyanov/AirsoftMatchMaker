using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IAmmoBoxService
    {
        /// <summary>
        /// Checks if <see cref="AmmoBox"/> with given id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> AmmoBoxExistsAsync(int id);

        /// <summary>
        /// Returns true if the user is selling the <see cref="AmmoBox"/>is not the same as the one 
        /// who is trying to  buy it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ammoBoxId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserCanBuyAmmoBoxAsync(string userId, int ammoBoxId);

        /// <summary>
        /// If the user does not have enough credits returns false 
        /// (if user is a player his upcoming games entry fee is also calculated)
        /// ,otherwise returns true.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ammoBoxId"></param>
        /// <param name="quantity"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserHasEnoughCreditsAsync(string userId, int ammoBoxId, int quantity);

        /// <summary>
        /// Returns <see cref="AmmoBoxesQueryModel"/> filled with ammo boxes which match the given criteria.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="sorting"></param>
        /// <param name="ammoBoxesPerPage"></param>
        /// <param name="currentPage"></param>
        /// <returns><see cref="AmmoBoxesQueryModel"/></returns>
        Task<AmmoBoxesQueryModel> GetAllAmmoBoxesAsync(
            string? searchTerm,
            AmmoBoxSorting sorting,
            int ammoBoxesPerPage = 6,
            int currentPage = 1);

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

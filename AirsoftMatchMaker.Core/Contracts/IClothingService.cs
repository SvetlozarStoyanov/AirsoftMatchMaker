using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IClothingService
    {
        /// <summary>
        /// Checks if <see cref="Clothing clothing"/> with given id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> ClothingExistsAsync(int id);

        /// <summary>
        /// Returns true if the user is selling the <see cref="Clothing"/>is not the same as the one 
        /// who is trying to  buy it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clothingId"></param>
        /// <returns><see cref="bool"/><see cref="bool"/></returns>
        Task<bool> UserCanBuyClothingAsync(string userId, int clothingId);

        /// <summary>
        /// Returns true if the user who owns the <see cref="Clothing"/>is not the same as the one 
        /// who is trying to buy it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clothingId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserCanSellClothingAsync(string userId, int clothingId);

        /// <summary>
        /// If the user does not have enough credits returns false 
        /// (if user is a player his upcoming games entry fee is also calculated)
        /// ,otherwise returns true.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clothingId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> UserHasEnoughCreditsAsync(string userId, int clothingId);


        /// <summary>
        /// Returns all clothes
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<ClothingListModel>> GetAllClothesAsync();

        /// <summary>
        /// Returns clothing with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClothingViewModel"/></returns>
        Task<ClothingViewModel> GetClothingByIdAsync(int id);

        /// <summary>
        /// Adds clothing to inventory of buyer , subtracting the clothing's price from his credits and adding them to the seller's balance
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="vendorId"></param>
        /// <param name="clothingId"></param>
        /// <returns></returns>
        Task BuyClothingAsync(string buyerId, int vendorId, int clothingId);

        /// <summary>
        /// Creates a <see cref="Clothing clothing"/> from <see cref="ClothingCreateModel model"/>
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateClothingAsync(string vendorUserId, ClothingCreateModel model);

        /// <summary>
        /// Creates a <see cref="ClothingSellModel"/> in order to sell a weapon.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClothingSellModel"/></returns>
        Task<ClothingSellModel> CreateClothingSellModelAsync(int id);

        /// <summary>
        /// Removes <see cref="Clothing"/> from <see cref="Player"/> inventory and lists it for sale with given price.
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task SellClothingAsync(string vendorUserId, ClothingSellModel model);
    }
}

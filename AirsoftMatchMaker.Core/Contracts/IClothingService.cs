using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IClothingService
    {
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
    }
}

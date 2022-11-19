using AirsoftMatchMaker.Core.Models.Clothes;

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
    }
}

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
    }
}

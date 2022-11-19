using AirsoftMatchMaker.Core.Models.Vendors;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IVendorService
    {
        /// <summary>
        /// Checks for vendor with given userId and marks them as active, 
        /// if there is no vendor with this id it creates a new one and marks them as active.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task GrantVendorRoleAsync(string userId);

        /// <summary>
        /// Checks for vendor with given userId and marks them as inactive,
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveFromVendorRoleAsync(string userId);

        /// <summary>
        /// Returns all vendors
        /// </summary>
        /// <returns><see cref="IEnumerable{typeof(VendorListModel)}"/></returns>
        Task<IEnumerable<VendorListModel>> GetAllVendorsAsync();

        /// <summary>
        /// Returns vendor by given id
        /// </summary>
        /// <paramt type="int" name="id"></param>
        /// <returns><see cref="VendorViewModel"/></returns>
        Task<VendorViewModel> GetVendorByIdAsync(int id);
    }
}

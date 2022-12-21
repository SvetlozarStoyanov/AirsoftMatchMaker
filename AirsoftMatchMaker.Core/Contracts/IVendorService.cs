using AirsoftMatchMaker.Core.Models.Vendors;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IVendorService
    {

        /// <summary>
        /// Returns <see cref="true"/> if vendor has enough credits to import the ammoBox, returns false otherwise.
        /// </summary>
        /// <param name="vendorUserId"></param>
        /// <param name="finalPrice"></param>
        /// <returns></returns>
        Task<bool> CheckIfVendorHasEnoughCreditsAsync(string vendorUserId, decimal finalPrice);

        /// <summary>
        /// Returns <see cref="Vendor.Id"/> of given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="int"/></returns>
        Task<int> GetVendorIdAsync(string userId);


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

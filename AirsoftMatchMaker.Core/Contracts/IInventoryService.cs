using AirsoftMatchMaker.Core.Models.Inventory;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IInventoryService
    {
        Task<InventoryPlayerIndexModel> GetPlayerItemsAsync(string userId);
        Task<InventoryVendorIndexModel> GetVendorItemsAsync(string userId);
    }
}

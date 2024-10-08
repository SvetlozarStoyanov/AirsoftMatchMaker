using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Inventory;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork unitOfWork;
        public InventoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<InventoryPlayerIndexModel> GetPlayerItemsAsync(string userId)
        {
            var model = new InventoryPlayerIndexModel();

            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .Include(p => p.Clothes)
                .Include(p => p.Weapons)
                .FirstOrDefaultAsync();

            if (player != null)
            {
                model.Ammo = player.Ammo;
                model.Clothes = player.Clothes
                    .Select(c => new ClothingListModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        Price = c.Price,
                        ClothingColor = c.ClothingColor
                    })
                    .ToList();
                model.Weapons = player.Weapons
                    .Select(w => new WeaponListModel()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        ImageUrl = w.ImageUrl,
                        Price = w.Price,
                        PreferedEngagementDistance = w.PreferedEngagementDistance,
                        WeaponType = w.WeaponType
                    })
                    .ToList();
            }
            return model;
        }

        public async Task<InventoryVendorIndexModel> GetVendorItemsAsync(string userId)
        {
            var model = new InventoryVendorIndexModel();
            var vendor = await unitOfWork.VendorRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .Include(p => p.AmmoBoxes)
                .Include(p => p.Clothes)
                .Include(p => p.Weapons)
                .FirstOrDefaultAsync();
            if (vendor != null)
            {
                model.AmmoBoxes = vendor.AmmoBoxes
                    .Select(ab => new AmmoBoxListModel()
                    {
                        Id = ab.Id,
                        Name = ab.Name,
                        Amount = ab.Amount,
                        Quantity = ab.Quantity,
                        VendorId= vendor.Id,
                        Price = ab.Price
                    })
                    .ToList();
                model.Clothes = vendor.Clothes
                    .Select(c => new ClothingListModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        Price = c.Price,
                        ClothingColor = c.ClothingColor
                    })
                    .ToList();
                model.Weapons = vendor.Weapons
                    .Select(w => new WeaponListModel()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        ImageUrl = w.ImageUrl,
                        Price = w.Price,
                        PreferedEngagementDistance = w.PreferedEngagementDistance,
                        WeaponType = w.WeaponType
                    })
                    .ToList();
            }
            return model;
        }
    }
}

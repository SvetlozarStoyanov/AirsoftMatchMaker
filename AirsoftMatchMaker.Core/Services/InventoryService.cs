using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Inventory;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository repository;
        public InventoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<InventoryPlayerIndexModel> GetPlayerItemsAsync(string userId)
        {
            var model = new InventoryPlayerIndexModel();
            var player = await repository.AllReadOnly<Player>()
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
            var vendor = await repository.AllReadOnly<Vendor>()
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

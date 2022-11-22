using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Vendors;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class VendorService : IVendorService
    {
        private readonly IRepository repository;
        public VendorService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task GrantVendorRoleAsync(string userId)
        {
            var vendor = await repository.All<Vendor>().FirstOrDefaultAsync(v => v.UserId == userId);
            if (vendor != null)
            {
                vendor.IsActive = true;
                await repository.SaveChangesAsync();
                return;
            }

            var newVendor = new Vendor()
            {
                UserId = userId,
            };
            //var player = await repository.All<Player>().FirstOrDefaultAsync(v => v.UserId == userId);
            //if (player != null)
            //{
            //    player.IsActive = false;
            //}
            await repository.AddAsync<Vendor>(newVendor);
            await repository.SaveChangesAsync();
        }

        public async Task RemoveFromVendorRoleAsync(string userId)
        {
            var vendor = await repository.All<Vendor>()
                .FirstOrDefaultAsync(v => v.UserId == userId);
            if (vendor == null)
            {
                return;
            }
            vendor.IsActive = false;
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<VendorListModel>> GetAllVendorsAsync()
        {
            var vendors = await repository.AllReadOnly<Vendor>()
                .Include(v => v.User)
                .Include(v => v.AmmoBoxes)
                .Include(v => v.Weapons)
                .Include(v => v.Clothes)
                .Select(v => new VendorListModel()
                {
                    Id = v.Id,
                    UserId = v.UserId,
                    UserName = v.User.UserName,
                    AmmoBoxesForSaleCount = v.AmmoBoxes.Sum(ab => ab.Quantity),
                    ClothesForSaleCount = v.Clothes.Count,
                    WeaponsForSaleCount = v.Weapons.Count,
                })
                .ToListAsync();
            return vendors;
        }

        public async Task<VendorViewModel> GetVendorByIdAsync(int id)
        {
            var vendor = await repository.AllReadOnly<Vendor>()
                .Where(v => v.Id == id)
                .Select(v => new VendorViewModel()
                {
                    Id = v.Id,
                    IsActive = v.IsActive,
                    UserId = v.UserId,
                    UserName = v.User.UserName,
                    AmmoBoxes = v.AmmoBoxes
                    .Where(ab => ab.Quantity > 0)
                    .Select(ab => new AmmoBoxMinModel()
                    {
                        Id = ab.Id,
                        Name = ab.Name,
                        Amount = ab.Amount,
                        Price = ab.Price,
                    }).ToList(),
                    Weapons = v.Weapons
                    .Select(w => new WeaponMinModel()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        ImageUrl = w.ImageUrl,
                        Price = w.Price,
                    }).ToList(),
                    Clothes = v.Clothes
                    .Select(w => new ClothingMinModel()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        ImageUrl = w.ImageUrl,
                        Price = w.Price,
                    }).ToList(),

                })
                .FirstOrDefaultAsync();
            return vendor;
        }


    }
}

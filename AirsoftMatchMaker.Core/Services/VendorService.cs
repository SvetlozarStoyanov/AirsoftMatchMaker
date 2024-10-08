using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Vendors;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class VendorService : IVendorService
    {
        private readonly IUnitOfWork unitOfWork;
        public VendorService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> CheckIfVendorHasEnoughCreditsAsync(string vendorUserId, decimal finalPrice)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(vendorUserId);
            if (user.Credits < finalPrice)
                return false;
            return true;
        }

        public async Task<int> GetVendorIdAsync(string userId)
        {
            var vendorId = await unitOfWork.VendorRepository.AllReadOnly()
                .Where(v => v.UserId == userId)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();

            return vendorId;
        }

        public async Task GrantVendorRoleAsync(string userId)
        {
            var vendor = await unitOfWork.VendorRepository.All().FirstOrDefaultAsync(v => v.UserId == userId);
            if (vendor != null)
            {
                vendor.IsActive = true;
                await unitOfWork.SaveChangesAsync();
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
            await unitOfWork.VendorRepository.AddAsync(newVendor);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveFromVendorRoleAsync(string userId)
        {
            var vendor = await unitOfWork.VendorRepository.All()
                .FirstOrDefaultAsync(v => v.UserId == userId);
            if (vendor == null)
            {
                return;
            }
            vendor.IsActive = false;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<VendorListModel>> GetAllVendorsAsync()
        {
            var vendors = await unitOfWork.VendorRepository.AllReadOnly()
                .Where(v => v.IsActive)
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
            var vendor = await unitOfWork.VendorRepository.AllReadOnly()
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

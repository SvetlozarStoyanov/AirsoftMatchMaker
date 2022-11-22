using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class ClothingService : IClothingService
    {
        private readonly IRepository repository;
        public ClothingService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ClothingListModel>> GetAllClothesAsync()
        {
            var clothes = await repository.AllReadOnly<Clothing>()
                .Where(c => c.PlayerId == null && c.VendorId != null)
                .Include(c => c.Vendor)
                .ThenInclude(c => c.User)
                .Select(c => new ClothingListModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ClothingColor = c.ClothingColor,
                    Price = c.Price,
                })
                .ToListAsync();
            return clothes;
        }

        public async Task<ClothingViewModel> GetClothingByIdAsync(int id)
        {
            var clothing = await repository.AllReadOnly<Clothing>()
                .Where(c => c.Id == id)
                .Include(c => c.Vendor)
                .ThenInclude(v => v.User)
                .Include(c => c.Player)
                .ThenInclude(p => p.User)
                .Select(c => new ClothingViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Price = c.Price,
                    ClothingColor = c.ClothingColor,
                    VendorId = c.VendorId,
                    VendorName = c.VendorId != null ? c.Vendor.User.UserName : null,
                    PlayerId = c.PlayerId,
                    PlayerName = c.PlayerId != null ? c.Player.User.UserName : null,
                }).FirstOrDefaultAsync();
            return clothing;
        }

        public async Task BuyClothingAsync(string buyerId, int vendorId, int clothingId)
        {
            var buyer = await repository.All<Player>()
                .Where(p => p.UserId == buyerId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            if (buyer == null)
            {
                return;
            }
            var vendor = await repository.All<Vendor>()
                .Where(v => v.Id == vendorId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                return;
            }
            var clothing = await repository.GetByIdAsync<Clothing>(clothingId);
            if (clothing == null)
            {
                return;
            }
            if (buyer.User.Credits < clothing.Price)
            {
                return;
            }
            buyer.User.Credits -= clothing.Price;
            vendor.User.Credits += clothing.Price;
            buyer.Clothes.Add(clothing);
            vendor.Clothes.Remove(clothing);
            await repository.SaveChangesAsync();
        }

        public async Task CreateClothingAsync(string vendorUserId, ClothingCreateModel model)
        {
            var vendor = await repository.All<Vendor>()
                .Where(v => v.UserId == vendorUserId && v.IsActive)
                .Include(v => v.User)
                .Include(v => v.Clothes)
                .FirstOrDefaultAsync();

            if (vendor == null)
            {
                return;
            }
            var clotting = new Clothing()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                ClothingColor = model.ClothingColor
            };
            vendor.Clothes.Add(clotting);
            //await repository.AddAsync<AmmoBox>(ammoBox);
            await repository.SaveChangesAsync();
        }
    }
}

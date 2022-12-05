using AirsoftMatchMaker.Core.Contracts;
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

        public async Task<bool> ClothingExistsAsync(int id)
        {
            if (await repository.GetByIdAsync<Clothing>(id) == null)
                return false;
            return true;
        }
        public async Task<bool> UserCanBuyClothingAsync(string userId, int clothingId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            var clothing = await repository.GetByIdAsync<Clothing>(clothingId);
            if (clothing.Vendor.UserId == player.UserId)
                return false;
            return true;
        }


        public async Task<bool> UserCanSellClothingAsync(string userId, int clothingId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            var clothing = await repository.GetByIdAsync<Clothing>(clothingId);
            if (clothing.PlayerId != player.Id)
                return false;

            return true;
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
                    Price = c.Price,
                    ImageUrl = c.ImageUrl,
                    ClothingColor = c.ClothingColor,
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
            var clothing = new Clothing()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                ClothingColor = model.ClothingColor
            };
            vendor.User.Credits -= model.FinalImportPrice;
            vendor.Clothes.Add(clothing);
            await repository.SaveChangesAsync();
        }


        public async Task<ClothingSellModel> CreateClothingSellModelAsync(int id)
        {
            var clothing = await repository.AllReadOnly<Clothing>()
                .Where(c => c.Id == id)
                .Select(c => new ClothingSellModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    ClothingColor = c.ClothingColor,
                    Price = c.Price,
                    OldPrice = c.Price
                })
                .FirstOrDefaultAsync();

            return clothing;
        }

        public async Task SellClothingAsync(string vendorUserId, ClothingSellModel model)
        {
            var vendor = await repository.AllReadOnly<Vendor>()
                .Where(v => v.UserId == vendorUserId)
                .FirstOrDefaultAsync();
            var clothing = await repository.GetByIdAsync<Clothing>(model.Id);

            clothing.Name = model.Name;
            clothing.Price = model.Price;
            clothing.Description = model.Description;
            clothing.ImageUrl = model.ImageUrl;
            clothing.PlayerId = null;
            clothing.VendorId = vendor.Id;
            await repository.SaveChangesAsync();
        }


    }
}

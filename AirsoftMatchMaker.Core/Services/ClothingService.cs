using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
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

            var clothing = await repository.AllReadOnly<Clothing>()
                .Where(c => c.Id == clothingId)
                .Include(c => c.Vendor)
                .FirstOrDefaultAsync();
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

        public async Task<bool> ClothingIsForSaleAsync(int id)
        {
            var clothing = await repository.GetByIdAsync<Clothing>(id);
            return clothing.VendorId != null;
        }

        public async Task<bool> UserHasEnoughCreditsAsync(string userId, int clothingId)
        {
            var player = await repository.AllReadOnly<Player>()
              .Where(p => p.UserId == userId)
              .Include(p => p.User)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamRed)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamBlue)
              .FirstOrDefaultAsync();

            var clothing = await repository.GetByIdAsync<Clothing>(clothingId);
            if (player == null)
            {
                var user = await repository.GetByIdAsync<User>(userId);
                if (clothing.Price > user.Credits)
                {
                    return false;
                }
                return true;
            }
            if (player.TeamId != null)
            {
                var gamesEntryFeeSum = player.Team.GamesAsTeamRed
                .Union(player.Team.GamesAsTeamBlue)
                .Where(g => g.GameStatus == GameStatus.Upcoming)
                .Sum(g => g.EntryFee);
                if (gamesEntryFeeSum + clothing.Price > player.User.Credits)
                    return false;
            }
            return true;

        }
        public async Task<ClothesQueryModel> GetAllClothesAsync(
            ClothingColor? clothingColor,
            ClothingSorting sorting = ClothingSorting.Newest,
            string? searchTerm = null,
            int clothesPerPage = 6,
            int currentPage = 1)
        {
            var clothes = await repository.AllReadOnly<Clothing>()
                .Where(c => c.PlayerId == null && c.VendorId != null)
                .ToListAsync();
            if (clothingColor != null)
            {
                clothes = clothes.Where(c => c.ClothingColor == clothingColor).ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                clothes = clothes.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (sorting)
            {
                case ClothingSorting.PriceAscending:
                    clothes = clothes.OrderBy(c => c.Price).ToList();
                    break;
                case ClothingSorting.PriceDescending:
                    clothes = clothes.OrderByDescending(c => c.Price).ToList();
                    break;
                case ClothingSorting.Alphabetically:
                    clothes = clothes.OrderBy(c => c.Name).ToList();
                    break;
                case ClothingSorting.Newest:
                    clothes = clothes.OrderByDescending(c => c.Id).ToList();
                    break;
                case ClothingSorting.Oldest:
                    clothes = clothes.OrderBy(c => c.Id).ToList();
                    break;
            }
            var filteredClothes = clothes
            .Skip((currentPage - 1) * clothesPerPage)
            .Take(clothesPerPage)
            .Select(c => new ClothingListModel()
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
                ClothingColor = c.ClothingColor,
            }).ToList();
            var model = CreateClothesQueryModel();
            model.Clothes = filteredClothes;
            model.ClothesCount = clothes.Count;
            return model;
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

        public async Task<ClothingListModel> GetClothingListModelForBuyAsync(int id)
        {
            var clothing = await repository.AllReadOnly<Clothing>()
                .Where(c => c.Id == id)
                .Select(c => new ClothingListModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price
                })
                .FirstOrDefaultAsync();
            return clothing;
        }

        public async Task BuyClothingAsync(string buyerId, int clothingId)
        {
            var buyer = await repository.All<Player>()
                .Where(p => p.UserId == buyerId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            var clothing = await repository.GetByIdAsync<Clothing>(clothingId);
            var vendor = await repository.All<Vendor>()
                .Where(v => v.Id == clothing.VendorId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
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

        private ClothesQueryModel CreateClothesQueryModel()
        {
            var model = new ClothesQueryModel();
            var colours = Enum.GetNames<ClothingColor>().Cast<string>().ToList();
            var modelColours = new List<string>()
            {
                "All"
            };
            modelColours.AddRange(colours);
            model.Colors = modelColours;
            model.SortingOptions = Enum.GetValues<ClothingSorting>().Cast<ClothingSorting>().ToList();

            return model;
        }
    }
}

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

        public async Task<IEnumerable<ClothingListModel>> GetAllClothesAsync()
        {
            var clothes = await repository.AllReadOnly<Clothing>()
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
    }
}

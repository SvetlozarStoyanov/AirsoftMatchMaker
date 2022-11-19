using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class AmmoBoxService : IAmmoBoxService
    {
        private readonly IRepository repository;
        public AmmoBoxService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<AmmoBoxListModel>> GetAllAmmoBoxesAsync()
        {
            var ammoBoxes = await repository.AllReadOnly<AmmoBox>()
                .Where(ab => ab.Quantity > 0)
                .Select(ab => new AmmoBoxListModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
                    Amount = ab.Amount,
                    Quantity = ab.Quantity,
                    Price = ab.Price,
                    VendorId = ab.VendorId,


                }).ToListAsync();
            return ammoBoxes;
        }

        public async Task<AmmoBoxViewModel> GetAmmoBoxByIdAsync(int id)
        {
            var ammoBox = await repository.AllReadOnly<AmmoBox>()
                .Where(ab => ab.Id == id)
                .Include(ab => ab.Vendor)
                .ThenInclude(ab => ab.User)
                .Select(ab => new AmmoBoxViewModel()
                {
                    Id = ab.Id,
                    Amount = ab.Amount,
                    Quantity = ab.Quantity,
                    Price = ab.Price,
                    VendorId = ab.VendorId,
                    VendorName = ab.Vendor != null ? ab.Vendor.User.UserName : null
                })
                .FirstOrDefaultAsync();

            return ammoBox;
        }
    }
}

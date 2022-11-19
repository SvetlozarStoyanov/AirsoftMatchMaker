using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly IRepository repository;
        public WeaponService(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<WeaponListModel>> GetAllWeaponsAsync()
        {
            var weapons = await repository.AllReadOnly<Weapon>()
                .Select(w => new WeaponListModel()
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    ImageUrl = w.ImageUrl,
                    Price = w.Price,
                    PreferedEngagementDistance = w.PreferedEngagementDistance,
                    WeaponType = w.WeaponType

                })
                .ToListAsync();
            return weapons;
        }

        public async Task<WeaponViewModel> GetWeaponByIdAsync(int id)
        {
            var weapon = await repository.AllReadOnly<Weapon>()
                .Where(w => w.Id == id)
                .Include(w => w.Player)
                .ThenInclude(w => w.User)
                .Include(w => w.Vendor)
                .ThenInclude(w => w.User)
                .Select(w => new WeaponViewModel()
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    ImageUrl = w.ImageUrl,
                    Price = w.Price,
                    PreferedEngagementDistance = w.PreferedEngagementDistance,
                    WeaponType = w.WeaponType,
                    FireRate = w.FireRate,
                    FeetPerSecond = w.FeetPerSecond,
                    AverageAmmoExpendedPerGame = w.AverageAmmoExpendedPerGame,
                    VendorId = w.VendorId,
                    VendorName = w.Vendor != null ? w.Vendor.User.UserName : null,
                    PlayerId = w.PlayerId,
                    PlayerName = w.Player != null ? w.Player.User.UserName : null,
                }).FirstOrDefaultAsync();
            return weapon;
        }
    }
}

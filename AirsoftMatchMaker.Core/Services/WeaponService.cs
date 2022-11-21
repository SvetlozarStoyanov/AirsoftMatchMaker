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
                .Where(w => w.PlayerId == null && w.VendorId != null)
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

        public async Task BuyWeaponAsync(string buyerId, int vendorId, int weaponId)
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
            var weapon = await repository.GetByIdAsync<Weapon>(weaponId);
            if (weapon == null)
            {
                return;
            }

            if (buyer.User.Credits < weapon.Price)
            {
                return;
            }
            buyer.User.Credits -= weapon.Price;
            vendor.User.Credits += weapon.Price;
            buyer.Weapons.Add(weapon);
            vendor.Weapons.Remove(weapon);
            await repository.SaveChangesAsync();
        }
    }
}

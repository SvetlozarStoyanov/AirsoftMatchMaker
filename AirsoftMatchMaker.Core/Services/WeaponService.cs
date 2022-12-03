using AirsoftMatchMaker.Core.Common.Constants;
using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
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

        public WeaponCreateModel CreateWeaponCreateModelByWeaponType(WeaponType weaponType)
        {
            WeaponCreateModel model = new WeaponCreateModel();
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    break;

                case WeaponType.Shotgun:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    break;
                case WeaponType.SubmachineGun:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    break;
                case WeaponType.AssaultRifle:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Medium;
                    break;
                case WeaponType.SniperRifle:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Long;
                    break;
                default:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    model.PreferedEngagementDistances = Enum.GetValues<PreferedEngagementDistance>();
                    break;
            }
            return model;
        }

        public async Task CreateWeaponAsync(string vendorUserId, WeaponCreateModel model)
        {
            var vendor = await repository.All<Vendor>()
                .Where(v => v.UserId == vendorUserId && v.IsActive)
                .Include(v => v.User)
                .Include(v => v.Weapons)
                .FirstOrDefaultAsync();
            if (vendor == null)
            {
                return;
            }
            var weapon = new Weapon()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                FeetPerSecond = model.FeetPerSecond,
                FireRate = model.FireRate,
                Price = model.Price,
                WeaponType = model.WeaponType,
                PreferedEngagementDistance = model.PreferedEngagementDistance,
                AverageAmmoExpendedPerGame = model.AverageAmmoExpendedPerGame,
            };
            vendor.User.Credits -= model.FinalImportPrice;
            vendor.Weapons.Add(weapon);
            await repository.SaveChangesAsync();
        }

       

        public IEnumerable<string> ValidateWeaponParameters(WeaponCreateModel model)
        {
            List<string> errors = new List<string>();
            switch (model.WeaponType)
            {
                case WeaponType.Pistol:
                    if (model.FeetPerSecond < WeaponConstants.PistolFeetPerSecondMin || model.FireRate > WeaponConstants.PistolFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.PistolFeetPerSecondMin}:{WeaponConstants.PistolFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.PistolFireRateMin && model.FireRate > WeaponConstants.PistolFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.PistolFireRateMin}:{WeaponConstants.PistolFireRateMax}]");
                    }
                    break;
                case WeaponType.Shotgun:
                    if (model.FeetPerSecond < WeaponConstants.ShotgunFeetPerSecondMin || model.FireRate > WeaponConstants.ShotgunFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.PistolFeetPerSecondMin}:{WeaponConstants.PistolFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.ShotgunFireRateMin || model.FireRate > WeaponConstants.ShotgunFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.ShotgunFireRateMin}:{WeaponConstants.ShotgunFireRateMax}]");
                    }
                    break;
                case WeaponType.SubmachineGun:
                    if (model.FeetPerSecond < WeaponConstants.SubmachineGunFeetPerSecondMin || model.FireRate > WeaponConstants.SubmachineGunFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.SubmachineGunFeetPerSecondMin}:{WeaponConstants.SubmachineGunFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.SubmachineGunFireRateMin || model.FireRate > WeaponConstants.SubmachineGunFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.SubmachineGunFireRateMin}:{WeaponConstants.SubmachineGunFireRateMax}]");
                    }
                    break;
                case WeaponType.AssaultRifle:
                    if (model.FeetPerSecond < WeaponConstants.AssaultRifleFeetPerSecondMin || model.FireRate > WeaponConstants.AssaultRifleFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.AssaultRifleFeetPerSecondMin}:{WeaponConstants.AssaultRifleFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.AssaultRifleFireRateMin || model.FireRate > WeaponConstants.AssaultRifleFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.AssaultRifleFireRateMin}:{WeaponConstants.AssaultRifleFireRateMax}]");
                    }
                    break;
                case WeaponType.SniperRifle:
                    if (model.FeetPerSecond < WeaponConstants.SniperRifleFeetPerSecondMin || model.FireRate > WeaponConstants.SniperRifleFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.SniperRifleFeetPerSecondMin}:{WeaponConstants.SniperRifleFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.SniperRifleFireRateMin || model.FireRate > WeaponConstants.SniperRifleFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.SniperRifleFireRateMin}:{WeaponConstants.SniperRifleFireRateMax}]");
                    }
                    break;
                default:
                    if (model.FeetPerSecond < WeaponConstants.HeavyFeetPerSecondMin || model.FireRate > WeaponConstants.HeavyFeetPerSecondMax)
                    {
                        errors.Add($"Invalid feet per second! Range [{WeaponConstants.HeavyFeetPerSecondMin}:{WeaponConstants.HeavyFeetPerSecondMax}]");
                    }
                    if (model.FireRate < WeaponConstants.HeavyFireRateMin || model.FireRate > WeaponConstants.HeavyFireRateMax)
                    {
                        errors.Add($"Invalid fire rate! Range [{WeaponConstants.HeavyFireRateMin}:{WeaponConstants.HeavyFireRateMax}]");
                    }
                    break;
            }
            return errors;
        }
    }
}

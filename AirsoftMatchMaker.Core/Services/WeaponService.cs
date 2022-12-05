﻿using AirsoftMatchMaker.Core.Common.Constants;
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

        public async Task<bool> WeaponExistsAsync(int id)
        {
            if (await repository.GetByIdAsync<Weapon>(id) == null)
                return false;
            return true;
        }

        public async Task<bool> UserCanBuyWeaponAsync(string userId, int weaponId)
        {
            var player = await repository.AllReadOnly<Player>()
                   .Where(p => p.UserId == userId)
                   .FirstOrDefaultAsync();

            var weapon = await repository.GetByIdAsync<Weapon>(weaponId);
            if (weapon.Vendor.UserId == player.UserId)
                return false;
            return true;
        }

        public async Task<bool> UserCanSellWeaponAsync(string userId, int weaponId)
        {
            var player = await repository.AllReadOnly<Player>()
                 .Where(p => p.UserId == userId)
                 .FirstOrDefaultAsync();

            var weapon = await repository.GetByIdAsync<Weapon>(weaponId);
            if (weapon.PlayerId != player.Id)
                return false;
            return true;
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
            WeaponCreateModel model = new WeaponCreateModel() 
            {
                WeaponType = weaponType
            };
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    model.MinFeetPerSecond = WeaponConstants.PistolMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.PistolMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.PistolMinFireRate;
                    model.MaxFireRate = WeaponConstants.PistolMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.PistolMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.PistolMaxAverageAmmoExpended;
                    break;
                case WeaponType.Shotgun:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    model.MinFeetPerSecond = WeaponConstants.ShotgunMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.ShotgunMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.ShotgunMinFireRate;
                    model.MaxFireRate = WeaponConstants.ShotgunMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.ShotgunMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.ShotgunMaxAverageAmmoExpended;
                    break;
                case WeaponType.SubmachineGun:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    model.MinFeetPerSecond = WeaponConstants.SubmachineGunMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.SubmachineGunMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.SubmachineGunMinFireRate;
                    model.MaxFireRate = WeaponConstants.SubmachineGunMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.SubmachineGunMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.SubmachineGunMaxAverageAmmoExpended;
                    break;
                case WeaponType.AssaultRifle:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Medium;
                    model.MinFeetPerSecond = WeaponConstants.AssaultRifleMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.AssaultRifleMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.AssaultRifleMinFireRate;
                    model.MaxFireRate = WeaponConstants.AssaultRifleMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.AssaultRifleMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.AssaultRifleMaxAverageAmmoExpended;
                    break;
                case WeaponType.SniperRifle:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Long;
                    model.MinFeetPerSecond = WeaponConstants.SniperRifleMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.SniperRifleMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.SniperRifleMinFireRate;
                    model.MaxFireRate = WeaponConstants.SniperRifleMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.SniperRifleMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.SniperRifleMaxAverageAmmoExpended;
                    break;
                case WeaponType.Heavy:
                    model.PreferedEngagementDistance = PreferedEngagementDistance.Short;
                    model.PreferedEngagementDistances = Enum.GetValues<PreferedEngagementDistance>();
                    model.MinFeetPerSecond = WeaponConstants.HeavyMinFeetPerSecond;
                    model.MaxFeetPerSecond = WeaponConstants.HeavyMaxFeetPerSecond;
                    model.MinFireRate = WeaponConstants.HeavyMinFireRate;
                    model.MaxFireRate = WeaponConstants.HeavyMaxFireRate;
                    model.MinAverageAmmoExpended = WeaponConstants.HeavyMinAverageAmmoExpended;
                    model.MaxAverageAmmoExpended = WeaponConstants.HeavyMaxAverageAmmoExpended;
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

        public async Task<WeaponSellModel> CreateWeaponSellModelAsync(int id)
        {
            var weapon = await repository.AllReadOnly<Weapon>()
                .Where(w => w.Id == id)
                .Select(w => new WeaponSellModel()
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    ImageUrl = w.ImageUrl,
                    Price = w.Price,
                    OldPrice = w.Price,
                    WeaponType = w.WeaponType
                })
                .FirstOrDefaultAsync();
            return weapon;
        }

        public async Task SellWeaponAsync(string vendorUserId, WeaponSellModel model)
        {
            var vendor = await repository.AllReadOnly<Vendor>()
                .Where(v => v.UserId == vendorUserId)
                .FirstOrDefaultAsync();

            var weapon = await repository.GetByIdAsync<Weapon>(model.Id);
            weapon.Name = model.Name;
            weapon.Description = model.Description;
            weapon.Price = model.Price;
            weapon.PlayerId = null;
            weapon.VendorId = vendor.Id;
            await repository.SaveChangesAsync();
        }
    }
}

﻿using AirsoftMatchMaker.Core.Common.Constants;
using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly IUnitOfWork unitOfWork;
        public WeaponService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> WeaponExistsAsync(int id)
        {
            if (await unitOfWork.WeaponRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }

        public async Task<bool> UserCanBuyWeaponAsync(string userId, int weaponId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                   .Where(p => p.UserId == userId)
                   .FirstOrDefaultAsync();

            var weapon = await unitOfWork.WeaponRepository.AllReadOnly()
                .Where(w => w.Id == weaponId)
                .Include(w => w.Vendor)
                .FirstOrDefaultAsync();
            if (weapon.Vendor.UserId == player.UserId)
                return false;
            return true;
        }

        public async Task<bool> UserCanSellWeaponAsync(string userId, int weaponId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                 .Where(p => p.UserId == userId)
                 .FirstOrDefaultAsync();

            var weapon = await unitOfWork.WeaponRepository.AllReadOnly()
                         .Where(w => w.Id == weaponId)
                         .Include(w => w.Vendor)
                         .FirstOrDefaultAsync();
            if (weapon.PlayerId != player.Id)
                return false;
            return true;
        }
        public async Task<bool> WeaponIsForSaleAsync(int id)
        {
            var weapon = await unitOfWork.WeaponRepository.GetByIdAsync(id);
            return weapon.VendorId != null;
        }

        public async Task<bool> UserHasEnoughCreditsAsync(string userId, int weaponId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamRed)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamBlue)
                .FirstOrDefaultAsync();

            var weapon = await unitOfWork.WeaponRepository.GetByIdAsync(weaponId);

            if (player == null)
            {
                var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
                if (weapon.Price > user.Credits)
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

                if (gamesEntryFeeSum + weapon.Price > player.User.Credits)
                    return false;
            }
            return true;
        }

        public async Task<WeaponsQueryModel> GetAllWeaponsAsync(
            WeaponType? weaponType = null,
            PreferedEngagementDistance? preferedEngagementDistance = null,
            WeaponSorting weaponSorting = WeaponSorting.PriceAscending,
            string? searchTerm = null,
            int weaponsPerPage = 6,
            int currentPage = 1)
        {
            var weapons = await unitOfWork.WeaponRepository.AllReadOnly()
                .Where(w => w.PlayerId == null && w.VendorId != null)
                .ToListAsync();

            if (weaponType != null)
            {
                weapons = weapons.Where(w => w.WeaponType == weaponType).ToList();
            }
            if (preferedEngagementDistance != null)
            {
                weapons = weapons.Where(w => w.PreferedEngagementDistance == preferedEngagementDistance).ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                weapons = weapons.Where(w => w.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (weaponSorting)
            {
                case WeaponSorting.PriceAscending:
                    weapons = weapons.OrderBy(w => w.Price).ToList();
                    break;
                case WeaponSorting.PriceDescending:
                    weapons = weapons.OrderByDescending(w => w.Price).ToList();
                    break;
                case WeaponSorting.RangeAscending:
                    weapons = weapons.OrderBy(w => w.PreferedEngagementDistance).ToList();
                    break;
                case WeaponSorting.RangeDescending:
                    weapons = weapons.OrderByDescending(w => w.PreferedEngagementDistance).ToList();
                    break;
                case WeaponSorting.Newest:
                    weapons = weapons.OrderByDescending(w => w.Id).ToList();
                    break;
                case WeaponSorting.Oldest:
                    weapons = weapons.OrderBy(w => w.Id).ToList();
                    break;
                case WeaponSorting.FeetPerSecond:
                    weapons = weapons.OrderByDescending(w => w.FeetPerSecond).ToList();
                    break;
                case WeaponSorting.FireRate:
                    weapons = weapons.OrderByDescending(w => w.FireRate).ToList();
                    break;
                case WeaponSorting.AverageAmmoSpent:
                    weapons = weapons.OrderBy(w => w.AverageAmmoExpendedPerGame).ToList();
                    break;
            }
            var filteredWeapons = weapons
                .Skip((currentPage - 1) * weaponsPerPage)
                .Take(weaponsPerPage)
                .Select(w => new WeaponListModel()
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    ImageUrl = w.ImageUrl,
                    Price = w.Price,
                    PreferedEngagementDistance = w.PreferedEngagementDistance,
                    WeaponType = w.WeaponType
                }).ToList();
            var queryModel = CreateWeaponsQueryModel();

            queryModel.WeaponsCount = weapons.Count();
            queryModel.Weapons = filteredWeapons;
            return queryModel;
        }

        public async Task<WeaponViewModel> GetWeaponByIdAsync(int id)
        {
            var weapon = await unitOfWork.WeaponRepository.AllReadOnly()
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

        public async Task<WeaponListModel> GetWeaponListModelForBuyAsync(int id)
        {
            var weapon = await unitOfWork.WeaponRepository.AllReadOnly()
                .Where(c => c.Id == id)
                .Select(c => new WeaponListModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price
                })
                .FirstOrDefaultAsync();
            return weapon;
        }

        public async Task BuyWeaponAsync(string buyerId, int weaponId)
        {
            var buyer = await unitOfWork.PlayerRepository.All()
                .Where(p => p.UserId == buyerId)
                .Include(p => p.User)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamRed)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamBlue)
                .FirstOrDefaultAsync();
            var weapon = await unitOfWork.WeaponRepository.GetByIdAsync(weaponId);

            var vendor = await unitOfWork.VendorRepository.All()
                .Where(v => v.Id == weapon.VendorId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();

            buyer.User.Credits -= weapon.Price;
            vendor.User.Credits += weapon.Price;
            buyer.Weapons.Add(weapon);
            vendor.Weapons.Remove(weapon);
            if (buyer.Team != null)
            {
                var buyerGames = buyer.Team.GamesAsTeamRed.Union(buyer.Team.GamesAsTeamBlue);
                foreach (var game in buyerGames.Where(g => g.GameStatus == GameStatus.Upcoming))
                {
                    game.OddsAreUpdated = false;
                }
            }
            await unitOfWork.SaveChangesAsync();
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
            var vendor = await unitOfWork.VendorRepository.All()
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
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<WeaponSellModel> CreateWeaponSellModelAsync(int id)
        {
            var weapon = await unitOfWork.WeaponRepository.AllReadOnly()
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
            var vendor = await unitOfWork.VendorRepository.AllReadOnly()
                .Where(v => v.UserId == vendorUserId)
                .FirstOrDefaultAsync();

            var weapon = await unitOfWork.WeaponRepository.GetByIdAsync(model.Id);
            weapon.Name = model.Name;
            weapon.Description = model.Description;
            weapon.Price = model.Price;
            weapon.PlayerId = null;
            weapon.VendorId = vendor.Id;
            await unitOfWork.SaveChangesAsync();
        }

        private WeaponsQueryModel CreateWeaponsQueryModel()
        {
            var model = new WeaponsQueryModel();
            var weaponTypes = Enum.GetNames(typeof(WeaponType)).Cast<string>().ToList();

            List<string> weaponTypesForModel = new List<string>()
            {
                "All"
            };
            weaponTypesForModel.AddRange(weaponTypes);
            model.WeaponTypes = weaponTypesForModel;
            var preferedEngagementDistances = Enum.GetNames(typeof(PreferedEngagementDistance)).Cast<string>().ToList();

            List<string> preferedEngagementDistancesForModel = new List<string>()
            {
                "All"
            };
            preferedEngagementDistancesForModel.AddRange(preferedEngagementDistances);
            model.PreferedEngagementDistances = preferedEngagementDistancesForModel;
            model.SortingOptions = Enum.GetValues<WeaponSorting>().Cast<WeaponSorting>().ToList();
            return model;
        }

    }
}

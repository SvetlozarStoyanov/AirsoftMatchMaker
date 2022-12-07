using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
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

        public async Task<bool> AmmoBoxExistsAsync(int id)
        {
            if (await repository.GetByIdAsync<AmmoBox>(id) == null)
                return false;
            return true;
        }

        public async Task<bool> UserCanBuyAmmoBoxAsync(string userId, int ammoBoxId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            var weapon = await repository.AllReadOnly<AmmoBox>()
                .Where(w => w.Id == ammoBoxId)
                .Include(w => w.Vendor)
                .FirstOrDefaultAsync();
            if (weapon.Vendor.UserId == player.UserId)
                return false;
            return true;
        }


        public async Task<bool> UserHasEnoughCreditsAsync(string userId, int ammoBoxId, int quantity)
        {
            var player = await repository.AllReadOnly<Player>()
              .Where(p => p.UserId == userId)
              .Include(p => p.User)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamRed)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamBlue)
              .FirstOrDefaultAsync();
            var ammoBox = await repository.GetByIdAsync<AmmoBox>(ammoBoxId);
            if (player == null)
            {
                var user = await repository.GetByIdAsync<User>(userId);
                if (ammoBox.Price * quantity > user.Credits)
                {
                    return false;
                }
                return true;
            }
            var gamesEntryFeeSum = player.Team.GamesAsTeamRed
                .Union(player.Team.GamesAsTeamBlue)
                .Where(g => g.GameStatus == GameStatus.Upcoming)
                .Sum(g => g.EntryFee);

            if (gamesEntryFeeSum + ammoBox.Price * quantity > player.User.Credits)
                return false;

            return true;
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

        public async Task<AmmoBoxBuyModel> GetAmmoBoxToBuyByIdAsync(int id)
        {
            var ammoBox = await repository.AllReadOnly<AmmoBox>()
                .Where(ab => ab.Id == id)
                .Include(ab => ab.Vendor)
                .Select(ab => new AmmoBoxBuyModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
                    AmmoAmount = ab.Amount,
                    Price = ab.Price,
                    Quantity = ab.Quantity,
                    VendorId = ab.VendorId.Value,
                    VendorName = ab.Vendor.User.UserName,
                })
                .FirstOrDefaultAsync();

            return ammoBox;
        }

        public async Task BuyAmmoBoxAsync(string playerUserId, AmmoBoxBuyModel model)
        {
            var player = await repository.All<Player>()
                .Where(p => p.UserId == playerUserId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            if (player == null || player.User.Credits < model.Price * model.QuantityToBuy)
            {
                return;
            }
            var vendor = await repository.All<Vendor>()
                .Where(v => v.Id == model.VendorId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            if (player.UserId == vendor.UserId)
            {
                return;
            }
            if (vendor == null)
            {
                return;
            }
            player.User.Credits -= model.Price * model.QuantityToBuy;
            vendor.User.Credits += model.Price * model.QuantityToBuy;
            player.Ammo += model.AmmoAmount * model.QuantityToBuy;
            var ammoBox = await repository.GetByIdAsync<AmmoBox>(model.Id);
            ammoBox.Quantity -= model.QuantityToBuy;
            await repository.SaveChangesAsync();
        }



        public async Task CreateAmmoBoxAsync(string vendorUserId, AmmoBoxCreateModel model)
        {
            var vendor = await repository.All<Vendor>()
                .Where(v => v.UserId == vendorUserId && v.IsActive)
                .Include(v => v.User)
                .Include(v => v.AmmoBoxes)
                .FirstOrDefaultAsync();

            if (vendor == null)
            {
                return;
            }
            var ammoBox = new AmmoBox()
            {
                Name = model.Name,
                Amount = model.AmmoAmount,
                Price = model.Price,
                Quantity = model.Quantity,
            };
            vendor.User.Credits -= model.FinalImportPrice;
            vendor.AmmoBoxes.Add(ammoBox);
            //await repository.AddAsync<AmmoBox>(ammoBox);
            await repository.SaveChangesAsync();
        }

        public async Task<AmmoBoxRestockModel> GetAmmoBoxForRestockByIdAsync(int id)
        {
            var ammoBox = await repository.AllReadOnly<AmmoBox>()
                .Where(ab => ab.Id == id)
                .Select(ab => new AmmoBoxRestockModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
                    Quantity = ab.Quantity,
                    AmmoAmount = ab.Amount,
                    Price = ab.Price,
                    VendorId = ab.VendorId.Value
                })
                .FirstOrDefaultAsync();
            return ammoBox;
        }

        public async Task RestockAmmoBox(string vendorUserId, AmmoBoxRestockModel model)
        {
            var vendor = await repository.All<Vendor>()
                .Where(v => v.UserId == vendorUserId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            if (vendor.Id != model.VendorId)
            {
                return;
            }
            var ammoBox = await repository.GetByIdAsync<AmmoBox>(model.Id);
            if (ammoBox == null)
            {
                return;
            }
            vendor.User.Credits -= model.FinalImportPrice;
            ammoBox.Quantity += model.QuantityAdded;
            await repository.SaveChangesAsync();
        }
    }
}

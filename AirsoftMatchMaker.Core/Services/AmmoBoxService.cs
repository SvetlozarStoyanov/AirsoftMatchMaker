using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.AmmoBoxes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class AmmoBoxService : IAmmoBoxService
    {
        private readonly IUnitOfWork unitOfWork;
        public AmmoBoxService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> AmmoBoxExistsAsync(int id)
        {
            if (await unitOfWork.AmmoBoxRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }

        public async Task<bool> UserCanBuyAmmoBoxAsync(string userId, int ammoBoxId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            var ammoBox = await unitOfWork.AmmoBoxRepository.AllReadOnly()
                .Where(w => w.Id == ammoBoxId)
                .Include(w => w.Vendor)
                .FirstOrDefaultAsync();

            if (ammoBox.Vendor.UserId == player.UserId)
                return false;

            return true;
        }


        public async Task<bool> UserHasEnoughCreditsAsync(string userId, int ammoBoxId, int quantity)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
              .Where(p => p.UserId == userId)
              .Include(p => p.User)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamRed)
              .Include(p => p.Team)
              .ThenInclude(p => p.GamesAsTeamBlue)
              .FirstOrDefaultAsync();
            var ammoBox = await unitOfWork.AmmoBoxRepository.GetByIdAsync(ammoBoxId);
            if (player == null)
            {
                var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
                if (ammoBox.Price * quantity > user.Credits)
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
                if (gamesEntryFeeSum + ammoBox.Price * quantity > player.User.Credits)
                    return false;
            }
            return true;
        }

        public async Task<AmmoBoxesQueryModel> GetAllAmmoBoxesAsync(
            string? searchTerm,
            AmmoBoxSorting sorting,
            int ammoBoxesPerPage = 6,
            int currentPage = 1)
        {
            var ammoBoxes = await unitOfWork.AmmoBoxRepository.AllReadOnly()
                .Where(ab => ab.Quantity > 0)
                .ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                ammoBoxes = ammoBoxes.Where(ab => ab.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (sorting)
            {
                case AmmoBoxSorting.PriceAscending:
                    ammoBoxes = ammoBoxes.OrderBy(ab => ab.Price).ToList();
                    break;
                case AmmoBoxSorting.PriceDescending:
                    ammoBoxes = ammoBoxes.OrderByDescending(ab => ab.Price).ToList();
                    break;
                case AmmoBoxSorting.Alphabetically:
                    ammoBoxes = ammoBoxes.OrderBy(ab => ab.Name).ToList();

                    break;
                case AmmoBoxSorting.Newest:
                    ammoBoxes = ammoBoxes.OrderByDescending(ab => ab.Id).ToList();
                    break;
                case AmmoBoxSorting.Oldest:
                    ammoBoxes = ammoBoxes.OrderBy(ab => ab.Id).ToList();
                    break;
                case AmmoBoxSorting.AmmoAmount:
                    ammoBoxes = ammoBoxes.OrderByDescending(ab => ab.Amount).ToList();
                    break;
                case AmmoBoxSorting.Quantity:
                    ammoBoxes = ammoBoxes.OrderByDescending(ab => ab.Quantity).ToList();
                    break;
            }
            var filteredAmmoBoxes = ammoBoxes
                .Skip((currentPage - 1) * ammoBoxesPerPage)
                .Take(ammoBoxesPerPage)
                .Select(ab => new AmmoBoxListModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
                    Amount = ab.Amount,
                    Quantity = ab.Quantity,
                    Price = ab.Price,
                    VendorId = ab.VendorId,
                }).ToList();
            var model = CreateAmmoBoxesQueryModel();
            model.AmmoBoxes = filteredAmmoBoxes;
            model.AmmoBoxesCount = ammoBoxes.Count;
            return model;
        }


        public async Task<AmmoBoxViewModel> GetAmmoBoxByIdAsync(int id)
        {
            var ammoBox = await unitOfWork.AmmoBoxRepository.AllReadOnly()
                .Where(ab => ab.Id == id)
                .Include(ab => ab.Vendor)
                .ThenInclude(ab => ab.User)
                .Select(ab => new AmmoBoxViewModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
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
            var ammoBox = await unitOfWork.AmmoBoxRepository.AllReadOnly()
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
            var player = await unitOfWork.PlayerRepository.All()
                .Where(p => p.UserId == playerUserId)
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            var vendor = await unitOfWork.VendorRepository.All()
                .Where(v => v.Id == model.VendorId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();

            player.User.Credits -= model.Price * model.QuantityToBuy;
            vendor.User.Credits += model.Price * model.QuantityToBuy;
            player.Ammo += model.AmmoAmount * model.QuantityToBuy;

            var ammoBox = await unitOfWork.AmmoBoxRepository.GetByIdAsync(model.Id);
            ammoBox.Quantity -= model.QuantityToBuy;
            await unitOfWork.SaveChangesAsync();
        }



        public async Task CreateAmmoBoxAsync(string vendorUserId, AmmoBoxCreateModel model)
        {
            var vendor = await unitOfWork.VendorRepository.All()
                .Where(v => v.UserId == vendorUserId && v.IsActive)
                .Include(v => v.User)
                .Include(v => v.AmmoBoxes)
                .FirstOrDefaultAsync();

            var ammoBox = new AmmoBox()
            {
                Name = model.Name,
                Amount = model.AmmoAmount,
                Price = model.Price,
                Quantity = model.Quantity,
            };
            vendor.User.Credits -= model.FinalImportPrice;
            vendor.AmmoBoxes.Add(ammoBox);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<AmmoBoxRestockModel> GetAmmoBoxForRestockByIdAsync(int id)
        {
            var ammoBox = await unitOfWork.AmmoBoxRepository.AllReadOnly()
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
            var vendor = await unitOfWork.VendorRepository.All()
                .Where(v => v.UserId == vendorUserId)
                .Include(v => v.User)
                .FirstOrDefaultAsync();

            var ammoBox = await unitOfWork.AmmoBoxRepository.GetByIdAsync(model.Id);
            vendor.User.Credits -= model.FinalImportPrice;
            ammoBox.Quantity += model.QuantityAdded;

            await unitOfWork.SaveChangesAsync();
        }

        private AmmoBoxesQueryModel CreateAmmoBoxesQueryModel()
        {
            var model = new AmmoBoxesQueryModel();
            model.SortingOptions = Enum.GetValues<AmmoBoxSorting>().Cast<AmmoBoxSorting>().ToList();
            return model;
        }
    }
}

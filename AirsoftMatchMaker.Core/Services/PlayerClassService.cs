using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class PlayerClassService : IPlayerClassService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlayerClassService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> PlayerClassExists(int id)
        {
            return await unitOfWork.PlayerClassRepository.GetByIdAsync(id) != null;
        }

        public async Task<bool> IsPlayerAlreadyInPlayerClass(string userId, int playerClassId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (player.PlayerClassId == playerClassId)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<PlayerClassListModel>> GetAllPlayerClassesAsync()
        {
            var playerClasses = await unitOfWork.PlayerClassRepository.AllReadOnly()
                .Where(p => p.Id > 1)
                .Select(p => new PlayerClassListModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToListAsync();

            return playerClasses;
        }

        public async Task<int> GetPlayersPlayerClassIdByUserIdAsync(string userId)
        {
            var playerClassId = await unitOfWork.PlayerRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .Select(p => p.PlayerClassId)
                .FirstOrDefaultAsync();
            return playerClassId;
        }

        public async Task ChangePlayerClassAsync(string userId, int playerClassId)
        {
            var player = await unitOfWork.PlayerRepository.All()
                .FirstOrDefaultAsync(p => p.UserId == userId);
            player.PlayerClassId = playerClassId;
            await unitOfWork.SaveChangesAsync();
        }

        //public async Task RemovePlayerFromPlayerClassAsync(string userId, int playerClassId)
        //{
        //    var player = await repository.AllReadOnly<Player>()
        //        .FirstOrDefaultAsync(p => p.UserId == userId);
        //    player.PlayerClassId = 1;
        //    await repository.SaveChangesAsync();
        //}
    }
}

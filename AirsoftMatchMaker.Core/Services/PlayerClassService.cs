using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class PlayerClassService : IPlayerClassService
    {
        private readonly IRepository repository;

        public PlayerClassService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> PlayerClassExists(int id)
        {
            return await repository.GetByIdAsync<PlayerClass>(id) != null;
        }

        public async Task<bool> IsPlayerAlreadyInPlayerClass(string userId, int playerClassId)
        {
            var player = await repository.AllReadOnly<Player>()
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (player.PlayerClassId == playerClassId)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<PlayerClassListModel>> GetAllPlayerClassesAsync()
        {
            var playerClasses = await repository.AllReadOnly<PlayerClass>()
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
            var playerClassId = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Select(p => p.PlayerClassId)
                .FirstOrDefaultAsync();
            return playerClassId;
        }

        public async Task ChangePlayerClassAsync(string userId, int playerClassId)
        {
            var player = await repository.All<Player>()
                .FirstOrDefaultAsync(p => p.UserId == userId);
            player.PlayerClassId = playerClassId;
            await repository.SaveChangesAsync();
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

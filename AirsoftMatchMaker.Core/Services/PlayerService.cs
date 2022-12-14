using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.PlayerClasses;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository repository;
        public PlayerService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CanUserLeavePlayerRole(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamRed)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamBlue)
                .FirstOrDefaultAsync();
            if (player.Team == null)
                return true;
            if (player.Team.GamesAsTeamRed.Any(g => g.GameStatus == GameStatus.Upcoming) || player.Team.GamesAsTeamBlue.Any(g => g.GameStatus == GameStatus.Upcoming))
                return false;
            return true;
        }

        public async Task GrantPlayerRoleAsync(string userId)
        {

            var player = await repository.All<Player>().FirstOrDefaultAsync(p => p.UserId == userId);
            if (player != null)
            {
                player.IsActive = true;
                await repository.SaveChangesAsync();
                return;
            }

            var newPlayer = new Player()
            {
                UserId = userId,
            };
            await repository.AddAsync<Player>(newPlayer);
            await repository.SaveChangesAsync();
        }
        public async Task RemoveFromPlayerRoleAsync(string userId)
        {
            var player = await repository.All<Player>()
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (player == null)
            {
                return;
            }
            player.IsActive = false;
            await repository.SaveChangesAsync();
        }
        public async Task<IEnumerable<PlayerListModel>> GetAllPlayersAsync()
        {
            var players = await repository.AllReadOnly<Player>()
                .Where(p => p.IsActive == true)
                .Include(p => p.User)
                .Include(p => p.Team)
                .Include(p => p.Weapons)
                .Select(p => new PlayerListModel()
                {
                    Id = p.Id,
                    UserName = p.User.UserName,
                    SkillLevel = p.SkillLevel,
                    TeamName = p.TeamId != null ? p.Team.Name : "none",
                    WeaponsCount = p.Weapons.Count()
                })
                .OrderByDescending(p => p.SkillLevel)
                .ThenBy(p => p.UserName)
                .ToListAsync();
            return players;
        }

        public async Task<PlayerViewModel> GetPlayerByIdAsync(int id)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.Id == id)
                .Include(p => p.Weapons)
                .Include(p => p.User)
                .Include(p => p.PlayerClass)

                .Select(p => new PlayerViewModel()
                {
                    Id = p.Id,
                    UserName = p.User.UserName,
                    SkillLevel = p.SkillLevel,
                    TeamId = p.TeamId.Value,
                    TeamName = p.TeamId != null ? p.Team.Name : "none",
                    PlayerClassName = p.PlayerClass.Name,
                    Clothes = p.Clothes.Select(c => new ClothingMinModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        ClothingColor = c.ClothingColor
                    }).ToList(),
                    Weapons = p.Weapons.Select(w => new WeaponMinModel()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        ImageUrl = w.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return player;
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Enums;
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


        public async Task<bool> PlayerExistsAsync(int id)
        {
            var player = await repository.GetByIdAsync<Player>(id);
            return player != null;
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

        public async Task<int?> GetPlayersTeamIdAsync(int id)
        {
            var player = await repository.GetByIdAsync<Player>(id);
            return player.TeamId;
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

        public async Task<decimal> GetPlayersAvailableCreditsAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                .Where(p => p.UserId == userId)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamRed)
                .Include(p => p.Team)
                .ThenInclude(p => p.GamesAsTeamBlue)
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            var credits = player.User.Credits;
            if (player.TeamId != null)
            {
                var upcomingGames = player.Team.GamesAsTeamRed.Union(player.Team.GamesAsTeamBlue).Where(g => g.GameStatus == GameStatus.Upcoming);
                if (upcomingGames.Count() > 0)
                {
                    credits -= upcomingGames.Sum(g => g.EntryFee);
                    if (credits < 0)
                    {
                        credits = 0;
                    }
                }
            }
            return credits;
        }
        public async Task<int?> GetPlayersTeamIdAsync(string userId)
        {
            var player = await repository.AllReadOnly<Player>()
                            .Where(p => p.UserId == userId)
                            .FirstOrDefaultAsync();
            return player.TeamId;
        }
        public async Task<PlayersQueryModel> GetAllPlayersAsync(
            string? searchTerm = null,
            PlayerSorting sorting = PlayerSorting.Oldest,
            int playersPerPage = 12,
            int currentPage = 1
            )
        {
            var players = await repository.AllReadOnly<Player>()
                .Where(p => p.IsActive == true)
                .Include(p => p.User)
                .Include(p => p.Team)
                .Include(p => p.Weapons)
                .ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                players = players.Where(p => p.User.UserName.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            switch (sorting)
            {
                case PlayerSorting.Newest:
                    players = players.OrderByDescending(p => p.Id).ToList();
                    break;
                case PlayerSorting.Oldest:
                    players = players.OrderBy(p => p.Id).ToList();
                    break;
                case PlayerSorting.SkillLevelAscending:
                    players = players.OrderBy(p => p.SkillPoints).ToList();
                    break;
                case PlayerSorting.SkillLevelDescending:
                    players = players.OrderByDescending(p => p.SkillPoints).ToList();
                    break;
            }
            var filteredPlayers = players
                .Skip((currentPage - 1) * playersPerPage)
                .Take(playersPerPage)
                .Select(p => new PlayerListModel()
                {
                    Id = p.Id,
                    UserName = p.User.UserName,
                    SkillLevel = p.SkillLevel,
                    TeamName = p.TeamId != null ? p.Team.Name : "none",
                    WeaponsCount = p.Weapons.Count()
                })
                .ToList();
            PlayersQueryModel model = CreatePlayersQueryModel();
            model.PlayersCount = players.Count;
            model.Players = filteredPlayers;
            return model;
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
                        ImageUrl = w.ImageUrl,
                        WeaponType = w.WeaponType,
                        PreferedEngagementDistance = w.PreferedEngagementDistance
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return player;
        }

        private PlayersQueryModel CreatePlayersQueryModel()
        {
            var model = new PlayersQueryModel();
            model.SortingOptions = Enum.GetValues<PlayerSorting>().ToList();
            return model;
        }

        
    }
}

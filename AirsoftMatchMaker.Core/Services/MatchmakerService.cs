using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Matchmakers;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class MatchmakerService : IMatchmakerService
    {
        private readonly IRepository repository;
        public MatchmakerService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> GetMatchmakerIdAsync(string userId)
        {
            var matchmakerId = await repository.AllReadOnly<Matchmaker>()
                .Where(mm => mm.UserId == userId)
                .Select(mm => mm.Id)
                .FirstOrDefaultAsync();
            return matchmakerId;
        }

        public async Task CreateMatchmakerAsync(string userId)
        {
            var matchmaker = await repository.All<Matchmaker>()
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (matchmaker != null)
            {
                matchmaker.IsActive = true;
                await repository.SaveChangesAsync();
                return;
            }

            var newMatchmaker = new Matchmaker()
            {
                UserId = userId,
            };
            //var player = await repository.All<Player>().FirstOrDefaultAsync(p => p.UserId == userId);
            //if (player != null)
            //{
            //    player.IsActive = false;
            //}
            await repository.AddAsync<Matchmaker>(newMatchmaker);
            await repository.SaveChangesAsync();
        }

        public async Task RetireMatchmakerAsync(string userId)
        {
            var matchmaker = await repository.All<Matchmaker>()
                    .FirstOrDefaultAsync(p => p.UserId == userId);
            if (matchmaker == null)
            {
                return;
            }
            matchmaker.IsActive = false;
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatchmakerListModel>> GetAllMatchmakersAsync()
        {
            var matchmakers = await repository.AllReadOnly<Matchmaker>()
                    .Include(mm => mm.User)
                    .Select(mm => new MatchmakerListModel()
                    {
                        Id = mm.Id,
                        UserId = mm.UserId,
                        UserName = mm.User.UserName,
                        IsActive = mm.IsActive,
                        OrganisedGames = mm.OrganisedGames.Count
                    })
                    .ToListAsync();
            return matchmakers;
        }

        public async Task<MatchmakerViewModel> GetMatchmakerByIdAsync(int id)
        {
            var matchmaker = await repository.AllReadOnly<Matchmaker>()
                    .Where(mm => mm.Id == id)
                    .Select(mm => new MatchmakerViewModel()
                    {
                        Id = mm.Id,
                        IsActive = mm.IsActive,
                        UserId = mm.UserId,
                        UserName = mm.User.UserName,
                        OrganisedGames = mm.OrganisedGames.Select(g => new GameMinModel()
                        {
                            Id = g.Id,
                            Name = g.Name,
                            Result = g.Result != null ? g.Result : "0:0",
                            Date = g.Date.ToShortDateString(),
                            GameStatus = g.GameStatus,
                        })
                        .ToList()
                    })
                    .FirstOrDefaultAsync();
            return matchmaker;
        }


    }
}

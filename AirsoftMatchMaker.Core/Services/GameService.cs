using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Common.Repository;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;
        public GameService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<GameListModel>> GetAllGamesAsync()
        {
            var games = await repository.AllReadOnly<Game>()
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .Include(g => g.Matchmaker)
                .ThenInclude(mm => mm.User)
                .Select(g => new GameListModel()
                {
                    Id = g.Id,
                    Name = $"{g.TeamRed.Name} VS {g.TeamBlue.Name}",
                    GameStatus = g.GameStatus,
                    Date = g.Date.ToShortDateString(),
                    EntryFee = g.EntryFee,
                    GameModeId = g.GameModeId,
                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MatchmakerId = g.MatchmakerId,
                    MatchmakerName = g.Matchmaker.User.UserName,
                    Result = g.Result != null ? g.Result : $"{g.TeamRedPoints}:{g.TeamBluePoints}",
                })
                .ToListAsync();
            return games;
        }

        public async Task<GameViewModel> GetGameByIdAsync(int id)
        {
            var game = await repository.AllReadOnly<Game>()
                        .Where(g => g.Id == id)
                        .Include(g => g.TeamRed)
                        .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                        .Include(g => g.TeamBlue)
                        .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                        .Include(g => g.Matchmaker)

                        .ThenInclude(mm => mm.User)
                        .Select(g => new GameViewModel()
                        {
                            Id = g.Id,
                            Name = $"{g.TeamRed.Name} VS {g.TeamBlue.Name}",
                            GameStatus = g.GameStatus,
                            Date = g.Date.ToShortDateString(),
                            EntryFee = g.EntryFee,
                            GameModeId = g.GameModeId,
                            GameModeName = g.GameMode.Name,
                            MapId = g.MapId,
                            MapName = g.Map.Name,
                            MatchmakerId = g.MatchmakerId,
                            MatchmakerName = g.Matchmaker.User.UserName,
                            TeamRedId = g.TeamRedId,
                            TeamRedName = g.TeamRed.Name,
                            TeamRedPlayers = g.TeamRed.Players
                            .Select(p => new PlayerMinModel()
                            {
                                Id = p.Id,
                                UserName = p.User.UserName,
                                SkillLevel = p.SkillLevel
                            })
                            .ToList(),
                            TeamBlueId = g.TeamBlueId,
                            TeamBlueName = g.TeamBlue.Name,
                            TeamBluePlayers = g.TeamBlue.Players
                            .Select(p => new PlayerMinModel()
                            {
                                Id = p.Id,
                                UserName = p.User.UserName,
                                SkillLevel = p.SkillLevel
                            })
                            .ToList(),
                            BetsForTeamRed = g.Bets.Count(b => b.WinningTeamId == g.TeamRedId),
                            BetsForTeamBlue = g.Bets.Count(b => b.WinningTeamId == g.TeamBlueId),
                            Result = g.Result != null ? g.Result : $"{g.TeamRedPoints}:{g.TeamBluePoints}",

                        })
                        .FirstOrDefaultAsync();
            return game;
        }
    }
}

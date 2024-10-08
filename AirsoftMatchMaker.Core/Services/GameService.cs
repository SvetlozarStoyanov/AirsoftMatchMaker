﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Teams;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork unitOfWork;
        public GameService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> GameExistsAsync(int id)
        {
            var game = await unitOfWork.GameRepository.GetByIdAsync(id);
            return game != null;
        }

        public async Task<bool> AreTeamPlayerCountsSimilarAsync(int teamRedId, int teamBlueId)
        {
            var teamRed = await unitOfWork.TeamRepository.AllReadOnly()
                .Where(t => t.Id == teamRedId)
                .Include(t => t.Players)
                .ThenInclude(p => p.Weapons)
                .FirstOrDefaultAsync();

            int teamRedPlayersCount = teamRed.Players.Count(p => p.Weapons.Any());

            var teamBlue = await unitOfWork.TeamRepository.AllReadOnly()
               .Where(t => t.Id == teamBlueId)
               .Include(t => t.Players)
               .ThenInclude(p => p.Weapons)
               .FirstOrDefaultAsync();

            int teamBluePlayersCount = teamBlue.Players.Count(p => p.Weapons.Any());

            return Math.Abs(teamRedPlayersCount - teamBluePlayersCount) <= 2;
        }

        public async Task<bool> GameCanBeFinalisedAsync(string userId, int gameId)
        {
            var game = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g => g.Id == gameId)
                .Include(g => g.Matchmaker)
                .ThenInclude(mm => mm.User)
                .FirstOrDefaultAsync();

            return game.Matchmaker.UserId == userId;
        }

        public async Task<bool> GameIsFinishedButNotFinalisedAsync(int id)
        {
            var game = await unitOfWork.GameRepository.AllReadOnly()
                 .Where(g => g.Id == id)
                 .Include(g => g.GameBetCreditsContainer)
                 .FirstOrDefaultAsync();

            return game.GameStatus == GameStatus.Finished && game.Result == null;
        }



        public async Task<GamesQueryModel> GetAllGamesAsync(
            string? teamName,
            string? gameModeName,
            GameStatus? gameStatus,
            GameSorting sorting,
            int gamesPerPage = 6,
            int currentPage = 1)
        {
            //var allGames = await repository.AllReadOnly<Game>()
            //    .ToListAsync();
            var games = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g =>
                ((!g.GameBetCreditsContainer.BetsArePaidOut && g.GameStatus == GameStatus.Upcoming)
                || (g.GameBetCreditsContainer.BetsArePaidOut && g.GameStatus == GameStatus.Finished))
                && (g.TeamRedOdds != 0 && g.TeamBlueOdds != 0))
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .Include(g => g.GameMode)
                .Include(g => g.Map)
                .ToListAsync();
            if (!string.IsNullOrEmpty(teamName))
            {
                games = games.Where(g => g.TeamRed.Name.ToLower() == teamName.ToLower() || g.TeamBlue.Name.ToLower() == teamName.ToLower()).ToList();
            }
            if (!string.IsNullOrEmpty(gameModeName))
            {
                games = games.Where(g => g.GameMode.Name.ToLower() == gameModeName.ToLower()).ToList();
            }
            if (gameStatus != null)
            {
                games = games.Where(g => g.GameStatus == gameStatus).ToList();
            }
            switch (sorting)
            {
                case GameSorting.Newest:
                    games = games.OrderByDescending(g => g.Date).ToList();
                    break;
                case GameSorting.Oldest:
                    games = games.OrderBy(g => g.Date).ToList();
                    break;
            }

            var filteredGames = games
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage)
                .Select(g => new GameListModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    GameStatus = g.GameStatus,
                    Date = g.Date,
                    Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",

                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MapImageUrl = g.Map.ImageUrl,
                    TerrainType = g.Map.Terrain,
                    TeamRedId = g.TeamRedId,
                    TeamRedName = g.TeamRed.Name,
                    IsAcceptingBets = g.Date.Date > DateTime.Now,
                    TeamBlueId = g.TeamBlueId,
                    TeamBlueName = g.TeamBlue.Name,
                    Result = g.Result != null ? g.Result : "Not played yet",
                })
                .ToList();
            GamesQueryModel model = await CreateGamesQueryModel();
            model.GamesCount = games.Count;
            model.Games = filteredGames;
            return model;
        }



        public async Task<GameViewModel> GetGameByIdAsync(int id)
        {
            var game = await unitOfWork.GameRepository.AllReadOnly()
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
                            Name = g.Name,
                            GameStatus = g.GameStatus,
                            Date = g.Date.ToShortDateString(),
                            EntryFee = g.EntryFee,

                            Map = new MapWithGameModeModel()
                            {
                                Id = g.MapId,
                                Name = g.Map.Name,
                                ImageUrl = g.Map.ImageUrl,
                                Terrain = g.Map.Terrain,
                                GameModeId = g.GameModeId,
                                GameModeName = g.GameMode.Name
                            },
                            MatchmakerId = g.MatchmakerId,
                            MatchmakerName = g.Matchmaker.User.UserName,
                            TeamRed = new TeamMinModel()
                            {
                                Id = g.TeamRedId,
                                Name = g.TeamRed.Name,
                                Wins = g.TeamRed.Wins,
                                Losses = g.TeamRed.Losses,
                                Odds = g.TeamRedOdds > 0 ? $"+{g.TeamRedOdds}" : $"{g.TeamRedOdds}"
                            },
                            TeamBlue = new TeamMinModel()
                            {
                                Id = g.TeamBlueId,
                                Name = g.TeamBlue.Name,
                                Wins = g.TeamBlue.Wins,
                                Losses = g.TeamBlue.Losses,
                                Odds = g.TeamBlueOdds > 0 ? $"+{g.TeamBlueOdds}" : $"{g.TeamBlueOdds}"
                            },
                            IsAcceptingBets = g.Date.Date > DateTime.Now,
                            Result = g.Result
                        })
                        .FirstOrDefaultAsync();
            return game;
        }


        public async Task<GameSelectDateModel> GetNextSevenAvailableDatesAsync()
        {
            DateTime dateTime = DateTime.Today.AddHours(36);
            HashSet<DateTime> availableDates = new HashSet<DateTime>();

            var maps = await unitOfWork.MapRepository.AllReadOnly()
                 .Include(m => m.GameMode)
                 .Include(m => m.Games)
                 .ToListAsync();

            var teams = await unitOfWork.TeamRepository.AllReadOnly()
                .Where(t => t.Players.Count > 0 && t.Players.Any(p => p.Weapons.Count > 0) && t.GamesAsTeamRed.All(g => g.GameStatus != GameStatus.Upcoming) && t.GamesAsTeamBlue.All(g => g.GameStatus != GameStatus.Upcoming))
                .Include(t => t.Players)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();
            int teamsCount = teams.Count;
            if (teams.Count < 2)
            {
                return null;
            }
            else if (teamsCount == 2 && Math.Abs(teams.First().Players.Count - teams.Last().Players.Count) > 2)
            {
                return null;
            }
            int i = 0;
            while (availableDates.Count < 7 && i < 30)
            {
                int mapCount = maps.Count;
                foreach (var map in maps)
                {
                    if (map.Games.Any(g => g.Date.Date == dateTime.Date))
                    {
                        mapCount--;
                    }
                }
                if (mapCount > 0)
                {
                    availableDates.Add(dateTime);
                }
                dateTime = dateTime.AddDays(1);
                i++;
            }
            if (availableDates.Count == 0)
            {
                return null;
            }
            var model = new GameSelectDateModel()
            {
                Dates = availableDates
                .Select(d => d.ToString())
                .ToList()
            };

            return model;
        }

        public async Task<GameCreateModel> CreateGameCreateModelAsync(string dateTimeString)
        {
            DateTime dateTime = DateTime.Parse(dateTimeString);
            var maps = await unitOfWork.MapRepository.AllReadOnly()
                .Where(m => m.Games.Any(g => g.Date.Date != dateTime.Date))
                .Include(m => m.GameMode)
                .Include(m => m.Games)
                .ToListAsync();
            //foreach (var map in maps)
            //{
            //    map.Games = map.Games.Where(g => g.GameStatus == GameStatus.Upcoming && g.Date.Day == dateTime.Date.Day).ToList();
            //}
            //maps = maps.Where(m => m.Games.Count == 0).ToList();

            var teams = await unitOfWork.TeamRepository.AllReadOnly()
                .Where(t => t.Players.Any(p => p.IsActive) && t.Players.Any(p => p.Weapons.Count > 0) && t.GamesAsTeamRed.All(g => g.Date.Date != dateTime.Date) && t.GamesAsTeamRed.All(g => g.Date.Date != dateTime.Date && g.GameStatus != GameStatus.Upcoming)
                    && t.GamesAsTeamBlue.All(g => g.Date.Date != dateTime.Date && g.GameStatus != GameStatus.Upcoming))
                .Include(t => t.Players)
                .ThenInclude(p => p.Weapons)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .ToListAsync();

            GameCreateModel model = new GameCreateModel()
            {
                DateString = dateTimeString,
                Maps = maps.Select(m => new MapSelectModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    GameModeId = m.GameModeId,
                    GameModeName = m.GameMode.Name
                }).ToList(),
                Teams = teams.Select(t => new TeamSelectModel()
                {
                    Id = t.Id,
                    Name = t.Players.Count > 1 ? $"{t.Name} ({t.Players.Count(p => p.Weapons.Any())} players)" : $"{t.Name} ({t.Players.Count} player)",
                    AverageSkillPoints = t.Players.Average(p => p.SkillPoints)
                }).ToList()
            };
            return model;
        }
        public async Task CreateGameAsync(string matchmakerUserId, GameCreateModel model)
        {
            var matchmaker = await unitOfWork.MatchmakerRepository.All()
                .FirstOrDefaultAsync(mm => mm.UserId == matchmakerUserId);

            var teamRed = await unitOfWork.TeamRepository.All()
                .Where(t => t.Id == model.TeamRedId)
                .Include(t => t.Players.Where(p => p.IsActive && p.Weapons.Any()))
                .ThenInclude(t => t.Weapons)
                .Include(t => t.GamesAsTeamRed)
                .Include(t => t.GamesAsTeamBlue)
                .FirstOrDefaultAsync();

            var teamBlue = await unitOfWork.TeamRepository.All()
              .Where(t => t.Id == model.TeamBlueId)
              .Include(t => t.Players.Where(p => p.IsActive && p.Weapons.Any()))
              .ThenInclude(t => t.Weapons)
              .Include(t => t.GamesAsTeamRed)
              .Include(t => t.GamesAsTeamBlue)
              .FirstOrDefaultAsync();

            DateTime dateTime = DateTime.Parse(model.DateString);


            var map = await unitOfWork.MapRepository.All()
                .Where(m => m.Id == model.MapId)
                .Include(m => m.GameMode)
                .FirstOrDefaultAsync();



            var game = new Game()
            {
                Name = $"{teamRed.Name} VS {teamBlue.Name}",
                EntryFee = model.EntryFee,
                GameStatus = GameStatus.Upcoming,
                Date = dateTime,
                Result = null,
                MatchmakerId = matchmaker.Id,
                MapId = model.MapId,
                GameModeId = map.GameModeId,
                TeamRedId = model.TeamRedId,
                TeamBlueId = model.TeamBlueId,
                TeamRedOdds = 0,
                TeamBlueOdds = 0,
                OddsAreUpdated = false
            };
            matchmaker.OrganisedGames.Add(game);
            await unitOfWork.SaveChangesAsync();
            var gameId = await unitOfWork.GameRepository.AllReadOnly()
                .OrderByDescending(g => g.Id)
                .Select(g => g.Id)
                .FirstOrDefaultAsync();
            var container = new GameBetCreditsContainer()
            {
                GameId = gameId,
                BetsArePaidOut = false
            };
            await unitOfWork.GameBetCreditsContainerRepository.AddAsync(container);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task<IEnumerable<GameListModel>> GetUpcomingGamesByDateAsync()
        {
            var games = await unitOfWork.GameRepository.AllReadOnly()
               .Where(g => g.GameStatus != GameStatus.Finished && (g.TeamRedOdds != 0 && g.TeamBlueOdds != 0))
               .Include(g => g.TeamRed)
               .Include(g => g.TeamBlue)
               .Include(g => g.Matchmaker)
               .ThenInclude(mm => mm.User)
               .Include(g => g.Map)
               .Select(g => new GameListModel()
               {
                   Id = g.Id,
                   Name = g.Name,
                   GameStatus = g.GameStatus,
                   Date = g.Date,
                   Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",
                   GameModeName = g.GameMode.Name,
                   MapId = g.MapId,
                   MapName = g.Map.Name,
                   MapImageUrl = g.Map.ImageUrl,
                   TerrainType = g.Map.Terrain,
                   TeamRedId = g.TeamRedId,
                   TeamRedName = g.TeamRed.Name,
                   IsAcceptingBets = g.Date.Date > DateTime.Now,
                   TeamBlueId = g.TeamBlueId,
                   TeamBlueName = g.TeamBlue.Name,
               })
               .ToListAsync();
            return games;

        }

        public async Task<IEnumerable<GameListModel>> GetPlayersLastFinishedAndFirstUpcomingGameAsync(string userId)
        {
            var player = await unitOfWork.PlayerRepository.AllReadOnly()
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            if (player.TeamId == null)
            {
                return new List<GameListModel>();
            }

            var team = await unitOfWork.TeamRepository.AllReadOnly()
                .Where(t => t.Id == player.TeamId)
                .FirstOrDefaultAsync();
            var games = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g => g.TeamRedId == team.Id || g.TeamBlueId == team.Id)
                .Include(g => g.Map)
                .Include(g => g.GameMode)
                .Include(g => g.TeamRed)
                .Include(t => t.TeamBlue)
                .Select(g => new GameListModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    GameStatus = g.GameStatus,
                    Date = g.Date,
                    Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",
                    GameModeId = g.GameModeId,
                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MapImageUrl = g.Map.ImageUrl,
                    TerrainType = g.Map.Terrain,
                    TeamRedId = g.TeamRedId,
                    TeamRedName = g.TeamRed.Name,
                    TeamBlueId = g.TeamBlueId,
                    TeamBlueName = g.TeamBlue.Name,
                    Result = g.Result != null ? g.Result : "Not played yet",
                })
                .ToListAsync();
            var playerGames = new List<GameListModel>();

            var lastGame = games
                .Where(g => g.GameStatus == GameStatus.Finished)
                .OrderByDescending(g => g.Date)
                .FirstOrDefault();
            if (lastGame != null)
            {
                playerGames.Add(lastGame);
            }
            var nextGame = games
                .Where(g => g.GameStatus == GameStatus.Upcoming)
                .OrderBy(g => g.Date)
                .FirstOrDefault();
            if (nextGame != null)
            {
                playerGames.Add(nextGame);
            }
            return playerGames;
        }

        public async Task<GamesMatchmakerQueryModel> GetAllGamesForAdminAndMatchmakerAsync(
            int? matchmakerId,
            MatchmakerGameStatus? status,
            GameSorting sorting,
            int gamesPerPage = 6,
            int currentPage = 1)
        {
            var games = await unitOfWork.GameRepository.AllReadOnly()
            .Include(g => g.TeamRed)
            .Include(g => g.TeamBlue)
            .Include(g => g.GameMode)
            .Include(g => g.Map)
            .Include(g => g.Matchmaker)
            .ThenInclude(mm => mm.User)
            .ToListAsync();
            if (matchmakerId != null)
            {
                games = games.Where(g => g.MatchmakerId == matchmakerId).ToList();
            }
            if (status != null)
            {
                switch (status)
                {
                    case MatchmakerGameStatus.Upcoming:
                        games = games.Where(g => g.GameStatus == GameStatus.Upcoming).ToList();
                        break;
                    case MatchmakerGameStatus.Finished:
                        games = games.Where(g => g.GameStatus == GameStatus.Finished && g.Result == null).ToList();
                        break;
                    case MatchmakerGameStatus.Finalized:
                        games = games.Where(g => g.GameStatus == GameStatus.Finished && g.Result != null).ToList();
                        break;
                }
            }

            switch (sorting)
            {
                case GameSorting.Newest:
                    games = games.OrderByDescending(g => g.Date).ToList();
                    break;
                case GameSorting.Oldest:
                    games = games.OrderBy(g => g.Date).ToList();
                    break;
            }

            var filteredGames = games
                .Skip((currentPage - 1) * gamesPerPage)
                .Take(gamesPerPage)
                .Select(g => new GameListModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    GameStatus = g.GameStatus,
                    Date = g.Date,
                    Odds = g.TeamRedOdds > 0 && g.TeamBlueOdds < 0 ? $"+{g.TeamRedOdds}:{g.TeamBlueOdds}" : g.TeamRedOdds < 0 && g.TeamBlueOdds > 0 ? $"{g.TeamRedOdds}:+{g.TeamBlueOdds}" : $"{g.TeamRedOdds}:{g.TeamBlueOdds}",

                    GameModeName = g.GameMode.Name,
                    MapId = g.MapId,
                    MapName = g.Map.Name,
                    MapImageUrl = g.Map.ImageUrl,
                    TerrainType = g.Map.Terrain,
                    TeamRedId = g.TeamRedId,
                    TeamRedName = g.TeamRed.Name,
                    IsAcceptingBets = g.Date.Date > DateTime.Now,
                    TeamBlueId = g.TeamBlueId,
                    TeamBlueName = g.TeamBlue.Name,
                    EntryFee = g.EntryFee,
                    Result = g.Result
                })
                .ToList();
            GamesMatchmakerQueryModel model = CreateMatchmakerGamesQueryModel();
            model.GamesCount = games.Count;
            model.Games = filteredGames;
            return model;
        }

        public async Task<GameFinaliseModel> CreateGameFinaliseModelAsync(int id)
        {
            var model = await unitOfWork.GameRepository.AllReadOnly()
                .Where(g => g.Id == id)
                .Select(g => new GameFinaliseModel()
                {
                    Id = g.Id,
                    GameModeMaxPoints = g.GameMode.PointsToWin,
                })
                .FirstOrDefaultAsync();


            return model;
        }

        public async Task FinalizeGameAsync(GameFinaliseModel model)
        {
            var game = await unitOfWork.GameRepository.All()
                .Where(g => g.Id == model.Id)
                .Include(g => g.Matchmaker)
                .ThenInclude(mm => mm.User)
                .Include(g => g.Map)
                .ThenInclude(m => m.GameMode)
                .Include(g => g.TeamRed)
                .Include(g => g.TeamBlue)
                .FirstOrDefaultAsync();

            game.TeamRedPoints = model.TeamRedPoints;
            game.TeamBluePoints = model.TeamBluePoints;

            var teamRed = game.TeamRed;


            var teamRedPlayers = await unitOfWork.PlayerRepository.All()
                .Where(p => p.TeamId.HasValue && p.TeamId == teamRed.Id && p.IsActive && p.Weapons.Any() && p.User.Credits >= game.EntryFee)
                .Include(p => p.User)
                .Include(p => p.Weapons)
                .ToListAsync();

            var teamBlue = game.TeamBlue;

            var teamBluePlayers = await unitOfWork.PlayerRepository.All()
            .Where(p => p.TeamId.HasValue && p.TeamId == teamBlue.Id && p.IsActive && p.Weapons.Any() && p.User.Credits >= game.EntryFee)
            .Include(p => p.User)
            .Include(p => p.Weapons)
            .ToListAsync();

            var map = game.Map;

            var gameMode = map.GameMode;

            var matchmaker = game.Matchmaker;

            if (model.TeamRedPoints > model.TeamBluePoints)
            {
                AwardWinningTeam(teamRed, teamRedPlayers, map);
                AwardLosingTeam(teamBlue, teamBluePlayers, map);
            }
            else if (model.TeamRedPoints < model.TeamBluePoints)
            {
                AwardLosingTeam(teamRed, teamRedPlayers, map);
                AwardWinningTeam(teamBlue, teamBluePlayers, map);
            }
            game.Result = $"{model.TeamRedPoints}:{model.TeamBluePoints}";

            int totalPlayerCount = teamRedPlayers.Count + teamBluePlayers.Count;
            TakeEntryFeeFromPlayers(teamRedPlayers, teamBluePlayers, matchmaker, game.EntryFee);
            await unitOfWork.SaveChangesAsync();
        }



        private async Task<GamesQueryModel> CreateGamesQueryModel()
        {
            var model = new GamesQueryModel();
            var gameStatuses = Enum.GetNames<GameStatus>().Cast<string>().ToList();
            var modelGameStatuses = new List<string>()
            {
                "All"
            };
            modelGameStatuses.AddRange(gameStatuses);
            model.GameStatuses = modelGameStatuses;
            var modelTeamNames = new List<string>()
            {
                "All"
            };
            modelTeamNames.AddRange(await unitOfWork.TeamRepository.AllReadOnly()
                .Select(t => t.Name)
                .ToListAsync());
            model.TeamNames = modelTeamNames;
            var modelGameModeNames = new List<string>()
            {
                "All"
            };
            modelGameModeNames.AddRange(await unitOfWork.GameModeRepository.AllReadOnly()
                .Select(t => t.Name)
                .ToListAsync());
            model.GameModeNames = modelGameModeNames;
            model.SortingOptions = Enum.GetValues<GameSorting>().ToList();
            return model;
        }

        private GamesMatchmakerQueryModel CreateMatchmakerGamesQueryModel()
        {
            var model = new GamesMatchmakerQueryModel();
            var gameStatuses = Enum.GetNames<MatchmakerGameStatus>().Cast<string>().ToList();
            var modelGameStatuses = new List<string>()
            {
                "All"
            };
            modelGameStatuses.AddRange(gameStatuses);
            model.MatchmakerGameStatuses = modelGameStatuses;
            model.SortingOptions = Enum.GetValues<GameSorting>().ToList();
            return model;
        }


        private void AwardWinningTeam(Team team, List<Player> players, Map map)
        {
            team.Wins += 1;
            foreach (var player in players)
            {
                var weaponUsed = DetermineWeaponUsed(player, map);
                var random = new Random();

                player.Ammo -= random.Next((int)(weaponUsed.AverageAmmoExpendedPerGame / 2), (int)weaponUsed.AverageAmmoExpendedPerGame * 2);

                if (player.Ammo < 0)
                    player.Ammo = 0;

                if (player.SkillPoints < 1000)
                {
                    player.SkillPoints += 50;
                    switch (player.SkillPoints)
                    {
                        case > 800:
                            player.SkillLevel = SkillLevel.Expert;
                            break;
                        case > 600:
                            player.SkillLevel = SkillLevel.Skilled;
                            break;
                        case > 300:
                            player.SkillLevel = SkillLevel.Intermediate;
                            break;
                    }
                }
            }
        }
        private void AwardLosingTeam(Team team, List<Player> players, Map map)
        {
            team.Losses += 1;
            foreach (var player in players)
            {
                var weaponUsed = DetermineWeaponUsed(player, map);

                var random = new Random();

                player.Ammo -= random.Next((int)(weaponUsed.AverageAmmoExpendedPerGame / 2), (int)weaponUsed.AverageAmmoExpendedPerGame * 2);

                if (player.Ammo < 0)
                    player.Ammo = 0;
                if (player.SkillPoints < 1000)
                {
                    player.SkillPoints += 25;
                    switch (player.SkillPoints)
                    {
                        case > 800:
                            player.SkillLevel = SkillLevel.Expert;
                            break;
                        case > 600:
                            player.SkillLevel = SkillLevel.Skilled;
                            break;
                        case > 300:
                            player.SkillLevel = SkillLevel.Intermediate;
                            break;
                    }
                }
            }
        }

        private void TakeEntryFeeFromPlayers(List<Player> teamRedPlayers, List<Player> teamBluePlayers, Matchmaker matchmaker, decimal entryFee)
        {
            var allPlayers = teamRedPlayers.Union(teamBluePlayers).ToList();
            foreach (var player in allPlayers)
            {
                player.User.Credits -= entryFee;
                matchmaker.User.Credits += entryFee;
                if (player.User.Credits < 0)
                {
                    matchmaker.User.Credits += player.User.Credits;
                    player.User.Credits = 0;
                }
            }
        }

        private Weapon DetermineWeaponUsed(Player player, Map map)
        {
            var weaponUsed = player.Weapons.FirstOrDefault(w => w.PreferedEngagementDistance.ToString() == map.AverageEngagementDistance.ToString());

            if (weaponUsed == null)
            {
                var random = new Random();
                weaponUsed = player.Weapons.ToArray()[random.Next(0, player.Weapons.Count - 1)];
            }
            return weaponUsed;
        }


    }
}

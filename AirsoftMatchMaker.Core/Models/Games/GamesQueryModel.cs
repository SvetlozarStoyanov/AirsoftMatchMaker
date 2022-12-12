using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GamesQueryModel
    {
        public GamesQueryModel()
        {
            TeamNames = new List<string>();
            SortingOptions = new List<GameSorting>();
            GameStatuses = new List<string>();
            Games = new HashSet<GameListModel>();
        }
        public int GamesPerPage { get; set; } = 6;
        public string? TeamName { get; set; }
        public string? GameModeName { get; set; }
        public GameStatus? GameStatus { get; set; }
        public GameSorting Sorting { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int GamesCount { get; set; }
        public ICollection<string> TeamNames { get; set; }
        public ICollection<string> GameModeNames { get; set; }
        public ICollection<GameSorting> SortingOptions { get; set; }
        public ICollection<string> GameStatuses { get; set; }
        public IEnumerable<GameListModel> Games { get; set; }
    }
}

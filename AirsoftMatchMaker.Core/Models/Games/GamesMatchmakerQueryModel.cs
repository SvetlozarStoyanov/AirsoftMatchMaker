using AirsoftMatchMaker.Core.Models.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GamesMatchmakerQueryModel
    {
        public GamesMatchmakerQueryModel()
        {
            SortingOptions = new List<GameSorting>();
            MatchmakerGameStatuses = new List<string>();
            Games = new HashSet<GameListModel>();
        }
        public int GamesPerPage { get; set; } = 6;
        public MatchmakerGameStatus? MatchmakerGameStatus { get; set; }
        public GameSorting Sorting { get; set; } = GameSorting.Oldest;
        public int CurrentPage { get; set; } = 1;
        public int GamesCount { get; set; }
        public int MatchmakerId { get; set; }
        public ICollection<GameSorting> SortingOptions { get; set; }
        public ICollection<string> MatchmakerGameStatuses { get; set; }
        public IEnumerable<GameListModel> Games { get; set; }
    }
}

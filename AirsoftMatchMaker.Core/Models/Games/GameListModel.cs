using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameListModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Date { get; set; } = null!;
        public decimal EntryFee { get; set; }
        public GameStatus GameStatus { get; set; }
        public int MapId { get; set; }
        public string MapName { get; set; } = null!;
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
        public int MatchmakerId { get; set; }
        public string MatchmakerName { get; set; } = null!;
        public string? Result { get; set; }
        public int TeamRedId { get; set; }
        public int TeamBlueId { get; set; }
    }
}

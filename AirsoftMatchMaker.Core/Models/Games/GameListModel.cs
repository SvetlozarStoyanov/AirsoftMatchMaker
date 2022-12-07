using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameListModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public GameStatus GameStatus { get; set; }

        public int MapId { get; set; }
        public string MapName { get; set; }
        public string? MapImageUrl { get; set; }
        public TerrainType TerrainType { get; set; }

        public string? Result { get; set; }
        public decimal EntryFee { get; set; }
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;

        public int TeamRedId { get; set; }
        public string TeamRedName { get; set; } = null!;
        public int TeamRedOdds { get; set; }

        public int TeamBlueId { get; set; }
        public string TeamBlueName { get; set; } = null!;
        public int TeamBlueOdds { get; set; }

    }
}

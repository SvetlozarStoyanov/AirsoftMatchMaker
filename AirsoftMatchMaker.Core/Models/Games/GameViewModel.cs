using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Games
{
    public class GameViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Date { get; set; }
        public decimal EntryFee { get; set; }
        public GameStatus GameStatus { get; set; }
        public int MapId { get; set; }
        public string MapName { get; set; } = null!;
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
        public int MatchmakerId { get; set; }
        public string MatchmakerName { get; set; }
        public string? Result { get; set; }

        public int TeamRedId { get; set; }
        public string TeamRedName { get; set; } = null!;
        public ICollection<PlayerMinModel> TeamRedPlayers { get; set; }
        public int TeamRedPoints { get; set; }
        public int TeamRedBets { get; set; }
        public int TeamRedOdds { get; set; }

        public int TeamBlueId { get; set; }
        public string TeamBlueName { get; set; } = null!;
        public ICollection<PlayerMinModel> TeamBluePlayers { get; set; }
        public int TeamBluePoints { get; set; }
        public int TeamBlueBets { get; set; }
        public int TeamBlueOdds { get; set; }

    }
}

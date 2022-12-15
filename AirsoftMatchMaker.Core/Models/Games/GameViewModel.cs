using AirsoftMatchMaker.Core.Models.Maps;
using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Core.Models.Teams;
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
        public string? Result { get; set; }

        public MapWithGameModeModel Map { get; set; }

        public int MatchmakerId { get; set; }
        public string MatchmakerName { get; set; }

        public TeamMinModel TeamRed { get; set; }

        public TeamMinModel TeamBlue { get; set; }

        public bool IsAcceptingBets { get; set; }

    }
}

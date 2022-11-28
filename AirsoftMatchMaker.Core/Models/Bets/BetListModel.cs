using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Bets
{
    public class BetListModel
    {
        [Key]
        public int Id { get; set; }
        public BetStatus BetStatus { get; set; } 
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int WinningTeamId { get; set; }
        public string ChosenTeamName { get; set; }
        public decimal CreditsBet { get; set; }
        public int Odds { get; set; }
    }
}

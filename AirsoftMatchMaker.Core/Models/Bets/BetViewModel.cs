using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Bets
{
    public class BetViewModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public BetStatus BetStatus { get; set; }
        public string GameName { get; set; }
        public int WinningTeamId { get; set; }
        public string WinningTeamName { get; set; }
        public decimal CreditsBet { get; set; }
        public decimal PotentialProfit { get; set; }
        public int Odds { get; set; }
    }
}

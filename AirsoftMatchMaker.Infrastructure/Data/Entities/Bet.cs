using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        public BetStatus BetStatus { get; set; } = BetStatus.Active;
        [ForeignKey(nameof(Entities.User.Id))]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        [ForeignKey(nameof(Entities.Game.Id))]
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        [ForeignKey(nameof(Entities.Team.Id))]
        public int WinningTeamId { get; set; }
        public decimal CreditsBet { get; set; }
        public int Odds { get; set; }
    }
}

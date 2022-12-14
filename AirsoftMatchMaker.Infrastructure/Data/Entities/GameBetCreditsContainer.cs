using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class GameBetCreditsContainer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Entities.Game.Id))]
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public decimal TeamRedCreditsBet { get; set; }
        public decimal TeamBlueCreditsBet { get; set; }
        public bool BetsArePaidOut { get; set; } = false;
    }
}

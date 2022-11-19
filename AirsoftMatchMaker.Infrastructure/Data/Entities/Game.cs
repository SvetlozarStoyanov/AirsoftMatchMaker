using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Game
    {
        public Game()
        {
            Bets = new HashSet<Bet>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = null!;
        [MaxLength(20)]
        public string? Result { get; set; }
        public DateTime Date { get; set; }
        public decimal EntryFee { get; set; }
        public GameStatus GameStatus { get; set; }

        [ForeignKey(nameof(Entities.Map.Id))]
        public int MapId { get; set; }
        public virtual Map Map { get; set; } = null!;

        [ForeignKey(nameof(Entities.GameMode.Id))]
        public int GameModeId { get; set; }
        public virtual GameMode GameMode { get; set; } = null!;

        [ForeignKey(nameof(Entities.Matchmaker.Id))]
        public int MatchmakerId { get; set; }
        public Matchmaker Matchmaker { get; set; }

        [ForeignKey(nameof(Entities.Team.Id))]
        public int TeamRedId { get; set; }
        [InverseProperty(nameof(Entities.Team.GamesAsTeamRed))]
        public virtual Team TeamRed { get; set; } = null!;
        public int TeamRedPoints { get; set; }

        [ForeignKey(nameof(Entities.Team.Id))]
        public int TeamBlueId { get; set; }
        [InverseProperty(nameof(Entities.Team.GamesAsTeamBlue))]
        public virtual Team TeamBlue { get; set; } = null!;
        public int TeamBluePoints { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}

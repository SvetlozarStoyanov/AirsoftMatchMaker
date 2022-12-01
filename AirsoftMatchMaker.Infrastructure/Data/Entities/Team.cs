using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            GamesAsTeamRed = new HashSet<Game>();
            GamesAsTeamBlue = new HashSet<Game>();
            TeamRequests = new HashSet<TeamRequest>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string Name { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }

        public virtual ICollection<TeamRequest> TeamRequests { get; set; }

        public virtual ICollection<Player> Players { get; set; } = null!;

        [InverseProperty(nameof(Entities.Game.TeamRed))]
        public virtual ICollection<Game> GamesAsTeamRed { get; set; }

        [InverseProperty(nameof(Entities.Game.TeamBlue))]
        public virtual ICollection<Game> GamesAsTeamBlue { get; set; }
    }
}
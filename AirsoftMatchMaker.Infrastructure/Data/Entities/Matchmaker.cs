using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Matchmaker
    {
        public Matchmaker()
        {
            OrganisedGames = new HashSet<Game>();
        }
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(Entities.User.Id))]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public virtual ICollection<Game> OrganisedGames { get; set; }
    }
}

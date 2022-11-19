using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class GameMode
    {
        public GameMode()
        {
            Maps = new HashSet<Map>();
            Games = new HashSet<Game>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        public int PointsToWin { get; set; }
        public virtual ICollection<Map> Maps { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}

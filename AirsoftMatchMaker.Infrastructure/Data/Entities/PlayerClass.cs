using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class PlayerClass
    {
        public PlayerClass()
        {
            Players = new HashSet<Player>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set; }
    }
}

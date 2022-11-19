using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Map
    {
        public Map()
        {
            Games = new HashSet<Game>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(60)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public Mapsize Mapsize { get; set; }
        public TerrainType Terrain { get; set; }
        public AverageEngagementDistance AverageEngagementDistance { get; set; }
        [ForeignKey(nameof(Entities.GameMode.Id))]
        public int GameModeId { get; set; }
        public virtual GameMode GameMode { get; set; } = null!;
        public virtual ICollection<Game> Games { get; set; }
    }
}

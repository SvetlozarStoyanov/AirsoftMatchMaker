using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapCreateModel
    {
        public MapCreateModel()
        {
            GameModeIds = new List<int>();
            GameModeNames = new List<string>();
            Mapsizes = new List<Mapsize>();
            TerrainTypes = new List<TerrainType>();
            AverageEngagementDistances = new List<AverageEngagementDistance>();
        }

        [Required]
        [MinLength(3), MaxLength(60)]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(5), MaxLength(200)]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public Mapsize Mapsize { get; set; }
        public TerrainType TerrainType { get; set; }
        public AverageEngagementDistance AverageEngagementDistance { get; set; }
        public int GameModeId { get; set; }
        public ICollection<string> GameModeNames { get; set; }
        public ICollection<int> GameModeIds { get; set; }
        public ICollection<Mapsize> Mapsizes { get; set; }
        public ICollection<TerrainType> TerrainTypes { get; set; }
        public ICollection<AverageEngagementDistance> AverageEngagementDistances { get; set; }
    }
}

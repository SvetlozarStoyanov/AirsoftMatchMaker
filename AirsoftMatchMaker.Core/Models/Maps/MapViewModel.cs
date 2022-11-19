using AirsoftMatchMaker.Core.Models.Games;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapViewModel
    {
        public MapViewModel()
        {
            Games = new HashSet<GameMinModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public Mapsize Mapsize { get; set; }
        public TerrainType Terrain { get; set; }
        public AverageEngagementDistance AverageEngagementDistance { get; set; }
       
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
        public virtual ICollection<GameMinModel> Games { get; set; }
    }
}

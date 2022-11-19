using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public Mapsize Mapsize { get; set; }
        public TerrainType Terrain { get; set; }
        public AverageEngagementDistance AverageEngagementDistance { get; set; }
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
        public int GamesPlayed { get; set; }
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapWithGameModeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public TerrainType Terrain { get; set; }
        public int GameModeId { get; set; }
        public string GameModeName { get; set; } = null!;
    }
}

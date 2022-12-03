using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class MapConfiguration : IEntityTypeConfiguration<Map>
    {
        public void Configure(EntityTypeBuilder<Map> builder)
        {
            builder.HasData(CreateMaps());
        }
        private List<Map> CreateMaps()
        {
            List<Map> maps = new List<Map>()
            {
                new Map
                {
                    Id = 1,
                    Name = "Forest",
                    Description = "Large forest map in Norway",
                    ImageUrl = "https://cdn.britannica.com/87/138787-050-33727493/Belovezhskaya-Forest-Poland.jpg",
                    Terrain = TerrainType.Forest,
                    AverageEngagementDistance = AverageEngagementDistance.Medium,
                    Mapsize = Mapsize.Large,
                    GameModeId = 1
                },
                new Map
                {
                    Id = 2,
                    Name = "Clear Field",
                    Description = "Small Field in California",
                    ImageUrl = "https://www.arboursabroad.com/westflanders_be_110318-56/",
                    Terrain = TerrainType.Field,
                    AverageEngagementDistance = AverageEngagementDistance.Short,
                    Mapsize = Mapsize.Small,
                    GameModeId = 2
                },
                new Map
                {
                    Id = 3,
                    Name = "Snow Villa",
                    Description = "Extra Large snowy map in Russia",
                    ImageUrl = "https://www.rukavillas.com/wp-content/uploads/2020/01/snowpalace-outside-800.jpg",
                    Terrain = TerrainType.Snowy,
                    AverageEngagementDistance = AverageEngagementDistance.Long,
                    Mapsize = Mapsize.ExtraLarge,
                    GameModeId = 2
                }
            };
            return maps;
        }
    }
}

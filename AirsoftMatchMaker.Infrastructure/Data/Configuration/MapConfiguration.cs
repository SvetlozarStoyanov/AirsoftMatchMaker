using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Name = "Bjorn Forest",
                    Description = "Large forest map in Norway",
                    ImageUrl = null,
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
                    ImageUrl = null,
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
                    ImageUrl = null,
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

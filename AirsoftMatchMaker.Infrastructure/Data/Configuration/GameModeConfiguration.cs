using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class GameModeConfiguration : IEntityTypeConfiguration<GameMode>
    {
        public void Configure(EntityTypeBuilder<GameMode> builder)
        {
            builder.HasData(CreateGameModes());
        }
        private List<GameMode> CreateGameModes()
        {
            List<GameMode> gameModes = new List<GameMode>()
            {
                new GameMode()
                {
                    Id = 1,
                    Name = "Capture the flag",
                    Description = "Whoever captures the flag first scores a point.",
                    PointsToWin = 3
                },
                new GameMode()
                {
                    Id = 2,
                    Name = "Secure area",
                    Description = "The team which controls the point in the middle for 5 minutes wins.",
                    PointsToWin = 2
                }
            };
            return gameModes;
        }
    }
}

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
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasData(CreateGames());
        }
        private List<Game> CreateGames()
        {
            List<Game> games = new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    Name = "First Game",
                    Date = DateTime.Now.AddHours(1),
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -100,
                    TeamBlueOdds = +100
                },
                new Game()
                {
                    Id = 2,
                    Name = "Rematch Game",
                    Date = DateTime.Now.AddDays(1),
                    EntryFee = 40,
                    MatchmakerId = 1,
                    GameModeId = 1,
                    MapId = 2,
                    TeamRedId = 2,
                    TeamBlueId = 1,
                    TeamRedOdds = +100,
                    TeamBlueOdds = -100,
                },
            };
            return games;
        }
    }
}

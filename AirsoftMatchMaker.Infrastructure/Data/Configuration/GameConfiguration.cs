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
                    Name = "Alpha vs Bravo",
                    Date = DateTime.Now.AddDays(-1),
                    EntryFee = 40,
                    GameModeId = 1,
                    MatchmakerId = 1,
                    MapId = 1,
                    TeamRedId = 1,
                    TeamBlueId = 2,
                    TeamRedOdds = -126,
                    TeamBlueOdds = +124,
                    GameBetCreditsContainerId = 1,
                    OddsAreUpdated = true,
                },
                //new Game()
                //{
                //    Id = 2,
                //    Name = "Bravo vs Alpha",
                //    Date = DateTime.Now.AddDays(1),
                //    EntryFee = 40,
                //    MatchmakerId = 1,
                //    GameModeId = 1,
                //    MapId = 2,
                //    TeamRedId = 2,
                //    TeamBlueId = 1,
                //    TeamRedOdds = +140,
                //    TeamBlueOdds = -140,
                //    GameBetCreditsContainerId = 2,
                //    OddsAreUpdated = false,
                //},
                //new Game()
                //{
                //    Id = 3,
                //    Name = "Charlie vs Delta",
                //    Date = DateTime.Now.AddDays(1),
                //    EntryFee = 40,
                //    MatchmakerId = 1,
                //    GameModeId = 1,
                //    MapId = 2,
                //    TeamRedId = 3,
                //    TeamBlueId = 4,
                //    TeamRedOdds = -160,
                //    TeamBlueOdds = +150,
                //    GameBetCreditsContainerId = 3,
                //    OddsAreUpdated = false,
                //},
            };
            return games;
        }
    }
}

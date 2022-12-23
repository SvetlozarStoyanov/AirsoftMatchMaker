using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    public class GameBetCreditsContainerConfiguration : IEntityTypeConfiguration<GameBetCreditsContainer>
    {
        public void Configure(EntityTypeBuilder<GameBetCreditsContainer> builder)
        {
            builder.HasData(CreateGameBetCreditsContainers());
        }

        private List<GameBetCreditsContainer> CreateGameBetCreditsContainers()
        {
            var gameBetCreditsContainers = new List<GameBetCreditsContainer>()
            {
                new GameBetCreditsContainer()
                {
                    Id = 1,
                    GameId = 1,
                    TeamRedCreditsBet = 50,
                    TeamBlueCreditsBet = 40
                },
                //new GameBetCreditsContainer()
                //{
                //    Id = 2,
                //    GameId = 2,
                //    TeamRedCreditsBet = 0,
                //    TeamBlueCreditsBet = 0
                //},
                //new GameBetCreditsContainer()
                //{
                //    Id = 3,
                //    GameId = 3,
                //    TeamRedCreditsBet = 0,
                //    TeamBlueCreditsBet = 0
                //}
            };
            return gameBetCreditsContainers;
        }
    }
}

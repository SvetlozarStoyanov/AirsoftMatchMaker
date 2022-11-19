using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    public class PlayerClassConfiguration : IEntityTypeConfiguration<PlayerClass>
    {
        public void Configure(EntityTypeBuilder<PlayerClass> builder)
        {
            builder.HasData(CreatePlayerClasses());
        }
        private List<PlayerClass> CreatePlayerClasses()
        {
            List<PlayerClass> playerClasses = new List<PlayerClass>()
            {
                new PlayerClass()
                {
                    Id = 1,
                    Name = "New Player",
                    Description = "New to the game. Prone to make mistakes."
                },
                new PlayerClass()
                {
                    Id = 2,
                    Name = "Leader",
                    Description = "Provides good advice and coordinates teams well."
                },
                new PlayerClass()
                {
                    Id = 3,
                    Name = "Frontline",
                    Description = "Always goes first. Good in both defence and offence."
                },
                new PlayerClass()
                {
                    Id = 4,
                    Name = "Marksman",
                    Description = "High accuracy over long range. Struggles in close range."
                },
                new PlayerClass()
                {
                    Id = 5,
                    Name = "Sneaky",
                    Description = "Loves to sneak behind and surprise enemy teams from behind."
                },
                new PlayerClass()
                {
                    Id = 6,
                    Name = "Camper",
                    Description = "Excels in defending, lacks in attacking."
                },
                new PlayerClass()
                {
                    Id = 7,
                    Name = "Rusher",
                    Description = "Excels in attacking, lacks in defending."
                }
            };


            return playerClasses;
        }
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(CreateTeams());
        }
        private List<Team> CreateTeams()
        {
            List<Team> teams = new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    Name = "Alpha",
                    Wins = 0,
                    Losses = 0
                },
                new Team()
                {
                    Id = 2,
                    Name = "Bravo",
                    Wins = 0,
                    Losses = 0
                },
                new Team()
                {
                    Id = 3,
                    Name = "Charlie",
                    Wins = 1,
                    Losses = 0
                },
                new Team()
                {
                    Id = 4,
                    Name = "Delta",
                    Wins = 0,
                    Losses = 1
                }
            };
            return teams;
        }
    }
}

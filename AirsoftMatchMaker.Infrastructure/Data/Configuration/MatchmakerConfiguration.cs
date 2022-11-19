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
    internal class MatchmakerConfiguration : IEntityTypeConfiguration<Matchmaker>
    {
        public void Configure(EntityTypeBuilder<Matchmaker> builder)
        {
            builder.HasData(CreateMatchmakers());
        }
        private List<Matchmaker> CreateMatchmakers()
        {
            List<Matchmaker> matchmakers = new List<Matchmaker>()
            {
                new Matchmaker()
                {
                    Id = 1,
                    UserId = "c5d9e543-7c2f-4345-a014-ebd860eef718",
                }
            };

            return matchmakers;
        }
    }
}

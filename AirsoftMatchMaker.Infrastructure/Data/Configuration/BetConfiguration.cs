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
    internal class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasData(CreateBets());
        }
        private List<Bet> CreateBets()
        {
            List<Bet> bets = new List<Bet>()
            {
                new Bet()
                {
                    Id = 1,
                    UserId = "b2451308-1197-4362-be78-f7ea7ca35fe9",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 1,
                    TeamRedRate = 1.25m,
                    TeamBlueRate = 0.65m,
                }
            };
            return bets;
        }
    }
}

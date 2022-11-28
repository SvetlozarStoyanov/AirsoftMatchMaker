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
                    UserId = "cc1cb39b-c0cf-41ed-856c-d3943aec605a",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 1,
                    Odds = -110
                },
                //new Bet()
                //{
                //    Id = 2,
                //    UserId = "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61",
                //    GameId = 1,
                //    CreditsBet = 10,
                //    WinningTeamId = 2,
                //    Odds = -150
                //}
            };
            return bets;
        }
    }
}

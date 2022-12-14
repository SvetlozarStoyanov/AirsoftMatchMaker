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
                    Odds = -122
                },
                new Bet()
                {
                    Id = 2,
                    UserId = "1f5be09b-2910-4ac0-8ff5-5c525ddf1b61",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 2,
                    Odds = +120
                },
                new Bet()
                {
                    Id = 3,
                    UserId = "799495ef-8794-491d-94d9-6bd37d51ba40",
                    GameId = 1,
                    CreditsBet = 20,
                    WinningTeamId = 2,
                    Odds = +122
                },
                new Bet()
                {
                    Id = 4,
                    UserId = "6f4bc586-751a-4a4b-8fec-4c7145b47a3e",
                    GameId = 1,
                    CreditsBet = 30,
                    WinningTeamId = 1,
                    Odds = -122
                }
            };

            return bets;
        }
    }
}

using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirsoftMatchMaker.Infrastructure.Data.Configuration
{
    public class TeamRequestConfiguration : IEntityTypeConfiguration<TeamRequest>
    {
        public void Configure(EntityTypeBuilder<TeamRequest> builder)
        {
            builder.HasData(CreateTeamRequests());
        }
        private List<TeamRequest> CreateTeamRequests()
        {
            var teamRequests = new List<TeamRequest>()
            {
                new TeamRequest()
                {
                    Id = 1,
                    PlayerId = 5,
                    TeamId = 1,
                    TeamRequestType = TeamRequestType.Join,
                    TeamRequestStatus = TeamRequestStatus.Accepted
                },
                new TeamRequest()
                {
                    Id = 2,
                    PlayerId = 6,
                    TeamId = 2,
                    TeamRequestType = TeamRequestType.Join,
                    TeamRequestStatus = TeamRequestStatus.Accepted
                },
            };

            return teamRequests;
        }
    }
}

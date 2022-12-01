using AirsoftMatchMaker.Core.Models.Players;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.TeamRequests
{
    public class TeamRequestListModel
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public PlayerMinModel Player { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public TeamRequestType TeamRequestType { get; set; }
        public TeamRequestStatus TeamRequestStatus { get; set; }
    }
}

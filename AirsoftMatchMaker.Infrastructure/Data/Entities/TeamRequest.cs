using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class TeamRequest
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Entities.Player.Id))]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        [ForeignKey(nameof(Entities.Team.Id))]
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public TeamRequestType TeamRequestType { get; set; }
        public TeamRequestStatus TeamRequestStatus { get; set; }
    }
}

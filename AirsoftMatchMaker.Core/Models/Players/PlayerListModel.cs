using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel;

namespace AirsoftMatchMaker.Core.Models.Players
{
    public class PlayerListModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        [DisplayName("Skill Level")]
        public SkillLevel SkillLevel { get; set; }
        [DisplayName("Weapons Count")]
        public int WeaponsCount { get; set; }
        [DisplayName("Team")]
        public string TeamName { get; set; } = null!;
        public PlayerStatus PlayerStatus { get; set; }
    }
}

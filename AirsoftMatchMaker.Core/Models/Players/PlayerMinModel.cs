using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel;

namespace AirsoftMatchMaker.Core.Models.Players
{
    public class PlayerMinModel
    {
        public int Id { get; set; }
        [DisplayName("User name")]
        public string UserName { get; set; } = null!;
        [DisplayName("Skill Level")]
        public SkillLevel SkillLevel { get; set; }
        public string PlayerClassName { get; set; }
    }
}

using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Weapons;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel;

namespace AirsoftMatchMaker.Core.Models.Players
{
    public class PlayerViewModel
    {
        public PlayerViewModel()
        {
            Clothes = new HashSet<ClothingMinModel>();
            Weapons = new HashSet<WeaponMinModel>();
        }
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public int? TeamId { get; set; }
        [DisplayName("Team Name")]
        public string? TeamName { get; set; }
        public SkillLevel SkillLevel { get; set; }

        public PlayerStatus PlayerStatus { get; set; }
        [DisplayName("Class")]
        public string PlayerClassName { get; set; } = null!;
        public virtual ICollection<ClothingMinModel> Clothes { get; set; }
        public virtual ICollection<WeaponMinModel> Weapons { get; set; }
    }
}

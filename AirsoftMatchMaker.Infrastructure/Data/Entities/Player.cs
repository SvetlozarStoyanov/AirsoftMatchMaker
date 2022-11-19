using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Player
    {
        public Player()
        {
            Weapons = new HashSet<Weapon>();
            Clothes = new HashSet<Clothing>();
        }
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public int Ammo { get; set; } = 200;
        [ForeignKey(nameof(Entities.User.Id))]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        
        [ForeignKey(nameof(Entities.Team.Id))]
        public int? TeamId { get; set; }
        public virtual Team? Team { get; set; }
        public SkillLevel SkillLevel { get; set; } = SkillLevel.Beginner;
        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.Idle;
        public int SkillPoints { get; set; } = 100;
        [ForeignKey(nameof(Entities.PlayerClass.Id))]
        public int PlayerClassId { get; set; } = 1;
        public virtual PlayerClass PlayerClass { get; set; }
        public virtual ICollection<Weapon> Weapons { get; set; }
        public virtual ICollection<Clothing> Clothes { get; set; }
    }
}

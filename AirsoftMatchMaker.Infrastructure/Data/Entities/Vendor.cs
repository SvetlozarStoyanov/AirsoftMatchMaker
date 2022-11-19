using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            AmmoBoxes = new HashSet<AmmoBox>();
            Clothes = new HashSet<Clothing>();
            Weapons = new HashSet<Weapon>();
        }
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(Entities.User.Id))]
        public string UserId { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<AmmoBox> AmmoBoxes { get; set; }
        public virtual ICollection<Clothing> Clothes { get; set; }
        public virtual ICollection<Weapon> Weapons { get; set; }
    }
}

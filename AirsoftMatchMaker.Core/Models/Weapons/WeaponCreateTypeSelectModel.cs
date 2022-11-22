using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponCreateTypeSelectModel
    {
        public WeaponCreateTypeSelectModel()
        {
            WeaponTypes = new HashSet<WeaponType>();
        }
        public WeaponType WeaponType { get; set; }
        public ICollection<WeaponType> WeaponTypes { get; set; }
    }
}

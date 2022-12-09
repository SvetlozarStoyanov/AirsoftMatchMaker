using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirsoftMatchMaker.Core.Models.Weapons
{
    public class WeaponsQueryModel
    {
        public WeaponsQueryModel()
        {
            WeaponTypes = new HashSet<string>();
            PreferedEngagementDistances = new HashSet<string>();
            Weapons = new HashSet<WeaponListModel>();
        }
        public WeaponType? WeaponType { get; set; }
        [Display(Name = "Range")]
        public PreferedEngagementDistance? PreferedEngagementDistance { get; set; }
        public WeaponSorting Sorting { get; set; }
        public string? SearchTerm { get; set; }
        public int WeaponsPerPage { get; set; } = 6;
        public int CurrentPage { get; set; } = 1;
        public int WeaponsCount { get; set; }
        public IEnumerable<string> WeaponTypes { get; set; }
        public IEnumerable<string> PreferedEngagementDistances { get; set; }
        public IEnumerable<WeaponSorting> SortingOptions { get; set; }
        public IEnumerable<WeaponListModel> Weapons { get; set; }

    }
}

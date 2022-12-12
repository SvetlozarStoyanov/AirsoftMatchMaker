using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Enums;

namespace AirsoftMatchMaker.Core.Models.Clothes
{
    public class ClothesQueryModel
    {
        public ClothesQueryModel()
        {
            Colors = new HashSet<string>();
            Clothes = new HashSet<ClothingListModel>();
            SortingOptions = new HashSet<ClothingSorting>();
        }
        public string? SearchTerm { get; set; }
        public ClothingColor? ClothingColor { get; set; }
        public ClothingSorting Sorting { get; set; } = ClothingSorting.Newest;
        public int ClothesPerPage { get; set; } = 6;
        public int CurrentPage { get; set; } = 1;
        public int ClothesCount { get; set; }
        public ICollection<string> Colors { get; set; }
        public ICollection<ClothingSorting> SortingOptions { get; set; }
        public IEnumerable<ClothingListModel> Clothes { get; set; }
    }
}

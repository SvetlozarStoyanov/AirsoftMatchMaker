using AirsoftMatchMaker.Core.Models.Clothes;
using AirsoftMatchMaker.Core.Models.Enums;
using AirsoftMatchMaker.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.AmmoBoxes
{
    public class AmmoBoxesQueryModel
    {
        public AmmoBoxesQueryModel()
        {
            AmmoBoxes = new HashSet<AmmoBoxListModel>();
            SortingOptions = new HashSet<AmmoBoxSorting>();
        }
        public string? SearchTerm { get; set; }
        public AmmoBoxSorting Sorting { get; set; } = AmmoBoxSorting.AmmoAmount;
        public int AmmoBoxesPerPage { get; set; } = 6;
        public int CurrentPage { get; set; } = 1;
        public int AmmoBoxesCount { get; set; }
        public ICollection<AmmoBoxSorting> SortingOptions { get; set; }
        public IEnumerable<AmmoBoxListModel> AmmoBoxes { get; set; }
    }
}

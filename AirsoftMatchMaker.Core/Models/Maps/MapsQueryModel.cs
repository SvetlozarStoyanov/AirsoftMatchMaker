using AirsoftMatchMaker.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.Maps
{
    public class MapsQueryModel
    {
        public MapsQueryModel()
        {
            GameModeNames = new List<string>();
            Maps = new HashSet<MapListModel>();
            SortingOptions = new List<MapSorting>();
        }
        public int MapsPerPage { get; set; } = 6;
        public int CurrentPage { get; set; } = 1;
        public string? SearchTerm { get; set; }
        public string? GameModeName { get; set; }
        public MapSorting Sorting { get; set; }
        public int MapsCount { get; set; }
        public ICollection<string> GameModeNames { get; set; }
        public ICollection<MapSorting> SortingOptions { get; set; }
        public IEnumerable<MapListModel> Maps { get; set; }
    }
}

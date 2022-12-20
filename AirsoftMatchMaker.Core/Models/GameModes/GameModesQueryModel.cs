using AirsoftMatchMaker.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.GameModes
{
    /// <summary>
    /// Model used for sorting/filtering game modes
    /// </summary>
    public class GameModesQueryModel
    {
        public GameModesQueryModel()
        {
            SortingOptions = new List<GameModeSorting>();
            GameModes = new HashSet<GameModeListModel>();
        }
        public int GameModesPerPage { get; set; } = 6;
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int GameModesCount { get; set; }
        public GameModeSorting Sorting { get; set; }
        public ICollection<GameModeSorting> SortingOptions { get; set; }
        public IEnumerable<GameModeListModel> GameModes { get; set; }
    }

}

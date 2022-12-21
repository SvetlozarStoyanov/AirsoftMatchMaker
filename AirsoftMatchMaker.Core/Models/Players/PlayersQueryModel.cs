using AirsoftMatchMaker.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.Players
{
        /// <summary>
        /// Model used for sorting/filtering players
        /// </summary>
    public class PlayersQueryModel
    {
        public PlayersQueryModel()
        {
            SortingOptions = new List<PlayerSorting>();
            Players = new HashSet<PlayerListModel>();
        }
        public int PlayersPerPage { get; set; } = 12;
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public PlayerSorting Sorting { get; set; }
        public int PlayersCount { get; set; }
        public ICollection<PlayerSorting> SortingOptions { get; set; }
        public IEnumerable<PlayerListModel> Players { get; set; }
    }
}

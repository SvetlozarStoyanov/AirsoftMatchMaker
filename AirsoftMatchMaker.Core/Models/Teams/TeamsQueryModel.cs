using AirsoftMatchMaker.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.Teams
{
    /// <summary>
    /// Model used for sorting/filtering teams
    /// </summary>
    public class TeamsQueryModel
    {
        public TeamsQueryModel()
        {
            SortingOptions = new List<TeamSorting>();
            Teams = new HashSet<TeamListModel>();
        }
        public int TeamsPerPage { get; set; } = 6;
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public TeamSorting Sorting { get; set; }
        public int TeamsCount { get; set; }
        public ICollection<TeamSorting> SortingOptions { get; set; }
        public IEnumerable<TeamListModel> Teams { get; set; }
    }
}

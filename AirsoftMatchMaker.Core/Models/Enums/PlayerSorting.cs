using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Models.Enums
{
    /// <summary>
    /// Enum used for sorting players 
    /// </summary>
    public enum PlayerSorting
    {
        /// <summary>
        /// Orders items by Id descending
        /// </summary>
        Newest,
        /// <summary>
        /// Orders items by Id ascending
        /// </summary>
        Oldest,
        /// <summary>
        /// Orders items by skill level ascending
        /// </summary>
        SkillLevelAscending,
        /// <summary>
        /// Orders items by skill level descending
        /// </summary>
        SkillLevelDescending,
    }
}

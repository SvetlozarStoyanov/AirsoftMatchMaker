namespace AirsoftMatchMaker.Core.Models.Enums
{
    /// <summary>
    /// Enum used for sorting maps 
    /// </summary>
    public enum MapSorting
    {
        /// <summary>
        /// Orders maps by Id descending
        /// </summary>
        Newest,
        /// <summary>
        /// Orders maps by Id ascending
        /// </summary>
        Oldest,
        /// <summary>
        /// Orders maps by number of games played ascending
        /// </summary>
        GamesPlayedAscending,
        /// <summary>
        /// Orders maps by number of games played descending
        /// </summary>
        GamesPlayedDescending,
    }
}

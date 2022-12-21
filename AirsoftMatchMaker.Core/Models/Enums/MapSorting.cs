namespace AirsoftMatchMaker.Core.Models.Enums
{
    /// <summary>
    /// Enum used for sorting maps 
    /// </summary>
    public enum MapSorting
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
        /// Orders items by number of games played ascending
        /// </summary>
        GamesPlayedAscending,
        /// <summary>
        /// Orders items by number of games played descending
        /// </summary>
        GamesPlayedDescending,
    }
}

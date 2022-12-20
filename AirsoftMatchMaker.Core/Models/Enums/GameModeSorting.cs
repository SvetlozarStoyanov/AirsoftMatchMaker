namespace AirsoftMatchMaker.Core.Models.Enums
{
    /// <summary>
    /// Enum used for sorting maps 
    /// </summary>
    public enum GameModeSorting
    {
        /// <summary>
        /// Orders items by id descending
        /// </summary>
        Newest,
        /// <summary>
        /// Orders items by id ascending
        /// </summary>
        Oldest,
        /// <summary>
        /// Orders items by games played ascending
        /// </summary>
        GamesPlayedAscending,
        /// <summary>
        /// Orders items by games played descending
        /// </summary>
        GamesPlayedDescending,
        /// <summary>
        /// Orders items by maps played ascending
        /// </summary>
        MapCountAscending,
        /// <summary>
        /// Orders items by maps played descending
        /// </summary>
        MapCountDescending
    }
}

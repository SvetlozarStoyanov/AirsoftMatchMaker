namespace AirsoftMatchMaker.Core.Models.Enums
{
    /// <summary>
    /// Enum used for sorting teams 
    /// </summary>
    public enum TeamSorting
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
        /// Orders by player count ascending
        /// </summary>
        PlayerCountAscending,
        /// <summary>
        /// Orders by player count descending
        /// </summary>
        PlayerCountDescending,
        /// <summary>
        /// Orders by number of wins
        /// </summary>
        Wins
    }
}

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBackgroundTeamService
    {
        /// <summary>
        /// Gets ids of team requests which can be fulfilled.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<int>> GetPlayersToAssignOrRemoveFromTeamsTeamRequestIdsAsync();

        /// <summary>
        /// Fulfills team requests with given ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task AssignOrRemovePlayerFromTeamByTeamRequestIdsAsync(IEnumerable<int> ids);
    }
}

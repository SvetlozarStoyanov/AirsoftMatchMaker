namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IBackgroundTeamService
    {

        Task<IEnumerable<int>> GetPlayersToAssignOrRemoveFromTeamsTeamRequestIdsAsync();


        Task AssignOrRemovePlayerFromTeamByTeamRequestIdsAsync(IEnumerable<int> ids);
    }
}

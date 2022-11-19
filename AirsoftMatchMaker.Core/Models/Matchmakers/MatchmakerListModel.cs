namespace AirsoftMatchMaker.Core.Models.Matchmakers
{
    public class MatchmakerListModel
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string UserId { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public int OrganisedGames { get; set; }
    }
}

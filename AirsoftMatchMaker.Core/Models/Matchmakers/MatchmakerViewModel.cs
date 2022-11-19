using AirsoftMatchMaker.Core.Models.Games;

namespace AirsoftMatchMaker.Core.Models.Matchmakers
{
    public class MatchmakerViewModel
    {
        public MatchmakerViewModel()
        {
            OrganisedGames = new HashSet<GameMinModel>();
        }
        public int Id { get; init; }
        public bool IsActive { get; init; } 
        public string UserId { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public virtual ICollection<GameMinModel> OrganisedGames { get; init; }
    }
}

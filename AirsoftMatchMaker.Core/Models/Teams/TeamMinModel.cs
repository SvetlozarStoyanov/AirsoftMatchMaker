using AirsoftMatchMaker.Core.Models.Players;

namespace AirsoftMatchMaker.Core.Models.Teams
{
    public class TeamMinModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Odds { get; set; }
    }
}

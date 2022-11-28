namespace AirsoftMatchMaker.Core.Models.Bets
{
    public class BetCreateModel
    {
        public string? UserId { get; set; }
        public int GameId { get; set; }
        public string? GameName { get; set; }

        public int TeamRedId { get; set; }
        public string? TeamRedName { get; set; }
        public int TeamBlueId { get; set; }
        public string? TeamBlueName { get; set; }
        public int WinningTeamId { get; set; }
        public decimal UserCredits { get; set; }
        public decimal CreditsPlaced { get; set; }
        public int TeamRedOdds { get; set; }
        public int TeamBlueOdds { get; set; }
    }
}
